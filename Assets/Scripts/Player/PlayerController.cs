
using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
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
public class PlayerController : MonoBehaviour, IDamageReceiver
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

    private WeaponsManager _weaponsManager;
    private IWeapon _currentWeapon;
    public TeamEnum TeamEnum { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
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
        _weaponsManager.CreateWeapons(playerWeapons);
        _currentWeapon = _weaponsManager.GetCurrentWeapon();
        //TODO change back to OnLMBPressed later, when will be working
        InputController.RegisterOnMouseLeftDown(UsePrimaryWeapon);
        InputController.RegisterOnMouseLeftUp(StopUsingPrimaryWeapon);
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
    /// Uses primary weapon.
    /// </summary>
    private void UsePrimaryWeapon()
    {
        _currentWeapon.UseWeapon(CalcDirectionVector(), TeamEnum);
    }
    /// <summary>
    /// User stopped using primary weapon.
    /// </summary>
    private void StopUsingPrimaryWeapon()
    {
        _currentWeapon.StopUsingWeapon();
    }
    // Update is called once per frame
    void Update()
    {
        if (!_isAlive)
        {
            return;
        }
        
        Vector3 axes = ReadAxes();
        MovePlayer(axes);
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

        float percentageHP = _currentHealth / _maxHealth;
        if (_currentHealth <= 0.0f)
        {
            _currentHealth = percentageHP = 0.0f;
            _isAlive = false;
            _roundEndedEvent.Invoke();
        }

        _guiManager.HealthBarController.UpdateHealthBar(percentageHP);
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
    }
}
