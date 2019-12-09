
using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Match;
using Assets.Scripts.Match.Entities;
using Assets.Scripts.Player;
using Assets.Scripts.Player.GUI;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Player.Weapons;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Author: Karol Kozuch
/// 
/// Default controller for the player.
/// </summary>
public class PlayerController : MonoBehaviour, IDamageReceiver, IPausable
{
    /// <summary>
    /// The main transform of the player hierarchy which will be used to move the player around.
    /// </summary>
    [SerializeField] private Transform _movingTransform;
    [SerializeField]
    private InputController _inputController;
    [SerializeField]
    private float _maxVelocity = 500f;
    [SerializeField]
    private float _maxHealth = 10000f;
    [SerializeField]
    private float _currentHealth;

    [SerializeField] private CollisionResolver _collisionResolver;

    private bool _isAlive = true;
    /// <summary>
    /// The mass of the player. Has influence on damage received by the player.
    /// </summary>
    [SerializeField] private float _mass = 1000.0f;
    /// <summary>
    /// How much energy is required to decrease the player's HP by 1 point?
    /// </summary>
    [SerializeField] private float _energyToHPRatio = 100.0f;
    /// <summary>
    /// Defines how much percent of hp will be taken away from the player upon hooking
    /// the wrong type of enemy/obstacle.
    /// </summary>
    [SerializeField] private float _wrongGloveUsedPenalty = .1f;
    /// <summary>
    /// Controller responsible for displaying player data to the user.
    /// </summary>
    [SerializeField] private GuiManager _guiManager;

    /// <summary>
    /// Callback to the match controller. Will be called when current round ends (player loses all hp).
    /// </summary>
    [SerializeField] private UnityEvent _roundEndedEvent;

    private Vector3 _currentVelocity;
    /// <summary>
    /// Starting position of the player, acquired upon initialization of the script. Do not change.
    /// </summary>
    private Vector3 _startingPosition;
    /// <summary>
    /// Set to true when execution of this monobehavior behavior is on hold.
    /// </summary>
    private bool _isPaused = false;
    /// <summary>
    /// Set to true when player tried to hook wrong tye of the object (for example energy block with physics glove).
    /// </summary>
    private bool _hasHookedWrongObject = false;
    private WeaponsManager _weaponsManager;
    private IWeapon _currentWeapon;
    public TeamEnum TeamEnum { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        MatchController.RegisterOnPause(Pause);
        MatchController.RegisterOnResume(Resume);
        _startingPosition = _movingTransform.position;
        ResetPlayer();
        //Invert the ratio in order to use multiplication instead of division later on. Faster calculations.
        _energyToHPRatio = 1.0f / _energyToHPRatio;
        _weaponsManager = GetComponentInChildren<WeaponsManager>();
        if (_weaponsManager == null)
        {
            throw new Exception("Error: weapon manager not found in player.");
        }

        if (_collisionResolver == null)
        {
            throw new Exception("Error: Collision Resolver not found in player!");
        }
        var playerWeapons = new List<WeaponData>();
        playerWeapons.Add(new WeaponData()
        {
            GloveType = GloveType.Glove,
            ProjectileType = ProjectileTypeEnum.Physical
        });
        playerWeapons.Add(new WeaponData()
        {
            GloveType = GloveType.Glove,
            ProjectileType = ProjectileTypeEnum.Energy
        });
        _weaponsManager.CreateWeapons(playerWeapons);
        _currentWeapon = _weaponsManager.GetCurrentWeapon();
        //TODO register VR controller counterparts
        InputController.RegisterOnMouseLeftDown(UsePrimaryWeapon);
        InputController.RegisterOnMouseLeftUp(StopUsingPrimaryWeapon);
        InputController.RegisterOnMouseRightPressed(SwitchPrimaryWeapon);

        InputController.RegisterOnTriggerDown(UsePrimaryWeapon);
        InputController.RegisterOnTriggerUp(StopUsingPrimaryWeapon);
        InputController.RegisterOnTouchPressed(SwitchPrimaryWeapon);
    }
    /// <summary>
    /// Reads horizontal and vertical axes, depending on whether is the game in editor mode or not.
    /// </summary>
    /// <returns></returns>
    Vector3 ReadAxes()
    {
        if (_inputController.ControllerDetected==false)
        {
            return _inputController.DefaultAxesValues;
        }

        var inputAxes = _inputController.TouchPadAxis;
        return new Vector3(inputAxes.x, 0, inputAxes.y);
    }
    /// <summary>
    /// Performs player movement.
    /// </summary>
    /// <param name="axes">Axes along which the movement shall be performed. Must be normalized.</param>
    void MovePlayer(Vector3 axes)
    {
        float timeDelta = Time.deltaTime;
        _currentVelocity = axes;
        _currentVelocity *= _maxVelocity;
        _currentVelocity *= timeDelta;

        //The editor itself applies correction to movement accordingly to player rotation,
        //but the deployed app does not. Apply it manually so the player will walk forward always
        //in the direction they're looking.
        if (_inputController.ControllerDetected)
        {
            _currentVelocity = VectorManipulator.RotateVector(transform.rotation, _currentVelocity);
        }
        
        Vector3 rotatedVelocity = VectorManipulator.RotateVector(transform.rotation, _currentVelocity);
        rotatedVelocity = _collisionResolver.SolveCollisions(_movingTransform.position, rotatedVelocity, _movingTransform.rotation);
        _currentVelocity = VectorManipulator.RotateVector(Quaternion.Inverse(_movingTransform.rotation), rotatedVelocity);
        
        _movingTransform.Translate(_currentVelocity);
    }
    /// <summary>
    /// Calculates the direction vector, where the player is currently looking.
    /// </summary>
    /// <returns></returns>
    private Vector3 CalcDirectionVector()
    {
        return transform.rotation * GameConstants.PlayerRotationRefVector;
    }
    /// <summary>
    /// Weapon switch callback. Switches the player's weapon.
    /// </summary>
    private void SwitchPrimaryWeapon()
    {
        if (_isPaused || !_isAlive)
        {
            return;
        }

        StopUsingPrimaryWeapon();
        _currentWeapon = _weaponsManager.GetNextWeapon();
    }
    /// <summary>
    /// Uses primary weapon.
    /// </summary>
    private void UsePrimaryWeapon()
    {
        if (_isPaused || !_isAlive || _hasHookedWrongObject)
        {
            return;
        }
        var usageResult = _currentWeapon.UseWeapon(CalcDirectionVector(), TeamEnum);
        if (usageResult == HookingResultEnum.WrongType)
        {
            ReceivePercentalDamage(_wrongGloveUsedPenalty);
            _hasHookedWrongObject = true;
        }
    }
    /// <summary>
    /// User stopped using primary weapon.
    /// </summary>
    private void StopUsingPrimaryWeapon()
    {
        _currentWeapon.StopUsingWeapon();
        _hasHookedWrongObject = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (_isPaused || !_isAlive)
        {
            return;
        }
        
        Vector3 axes = ReadAxes();
        MovePlayer(axes);
    }
    /// <summary>
    /// Called when the player runs out of health point..
    /// </summary>
    /// <returns></returns>
    private void PlayerDied()
    {
        _currentHealth = 0.0f;
        _isAlive = false;
        //_roundEndedEvent.Invoke();
    }
    /// <summary>
    /// Updates the info connected with player health.
    /// </summary>
    private void UpdatePlayerHealthState()
    {
        float percentageHp = GetHpPercentage();
        if (_currentHealth <= 0.0f)
        {
            percentageHp = 0.0f;
            PlayerDied();
        }

        _guiManager.HealthBarController.UpdateHealthBar(percentageHp);
    }
    /// <summary>
    /// Returns the ratio of current HP towards max HP.
    /// </summary>
    /// <returns></returns>
    private float GetHpPercentage()
    {
        return _currentHealth / _maxHealth;
    }
    /// <summary>
    /// Deals given percent of entire hp as damage to the player.
    /// </summary>
    /// <param name="percent">Percentage of full hp dealt as damage.</param>
    public void ReceivePercentalDamage(float percent)
    {
        if (_currentHealth <= 0.0f)
        {
            return;
        }
        
        _currentHealth -= _maxHealth * percent;

        UpdatePlayerHealthState();
    }
    /// <summary>
    /// Called when player shall receive damage.
    /// </summary>
    /// <param name="damage">Amount of damage.</param>
    public void ReceiveDamage(float damage)
    {
        if (_currentHealth <= 0.0f)
        {
            return;
        }

        damage *= _energyToHPRatio; //Scale the damage properly.
        _currentHealth -= damage;
        
        UpdatePlayerHealthState();
    }
    /// <summary>
    /// Returns kinetic data of the player.
    /// </summary>
    /// <returns></returns>
    public KineticObjectData GetKineticData()
    {
        return new KineticObjectData {Velocity = _currentVelocity, Mass = _mass};
    }
    /// <summary>
    /// Resets the player's stats.
    /// </summary>
    public void ResetPlayer()
    {
        _movingTransform.position = _startingPosition;
        _currentHealth = _maxHealth;
        _isAlive = true;
        _isPaused = false;
    }
    /// <summary>
    /// Causes the object to halt executing its behavior.
    /// </summary>
    public void Pause()
    {
        _isPaused = true;
    }
    /// <summary>
    /// Causes the object to resume executing its behavior.
    /// </summary>
    public void Resume()
    {
        _isPaused = false;
    }
}
