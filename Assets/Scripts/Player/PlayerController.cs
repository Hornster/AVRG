
using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Player.Weapons;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

/// <summary>
/// Author: Karol Kozuch
/// 
/// Default controller for the player.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputController _inputController;
    [SerializeField]
    private float _maxVelocity = 500f;

    private WeaponsManager _weaponsManager;
    private IWeapon _currentWeapon;
    public TeamEnum TeamEnum { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        _weaponsManager = GetComponentInChildren<WeaponsManager>();
        if (_weaponsManager == null)
        {
            throw new Exception("Error: weapon manager not found in player.");
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
        if (Application.isEditor)
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
        Vector3 currentVelocity = axes;
        currentVelocity *= _maxVelocity;
        currentVelocity *= timeDelta;

        //The editor itself applies correction to movement accordingly to player rotation,
        //but the deployed app does not. Apply it manually so the player will walk forward always
        //in the direction they're looking.
        if (Application.isEditor == false)
        {
            currentVelocity = VectorManipulator.RotateVector(transform.rotation, currentVelocity);
        }

        gameObject.transform.Translate(currentVelocity);
    }
    /// <summary>
    /// Calculates the direction vector, where the player is currently looking.
    /// </summary>
    /// <returns></returns>
    private Vector3 CalcDirectionVector()
    {
        return transform.rotation * Vector3.forward;
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
        Vector3 axes = ReadAxes();
        MovePlayer(axes);
    }
}
