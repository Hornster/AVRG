using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Player.Weapons;
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
    // Start is called before the first frame update
    void Start()
    {
        _weaponsManager = GetComponentInChildren<WeaponsManager>();
        if (_weaponsManager == null)
        {
            throw new Exception("Error: weapon manager not found in player.");
        }
    }

    Vector3 ReadAxes()
    {
        if (Application.isEditor)
        {
            return _inputController.DefaultAxesValues;
        }

        var inputAxes = _inputController.TouchPadAxis;
        return new Vector3(inputAxes.x, 0, inputAxes.y);
    }

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
    // Update is called once per frame
    void Update()
    {
        Vector3 axes = ReadAxes();
        MovePlayer(axes);
    }
}
