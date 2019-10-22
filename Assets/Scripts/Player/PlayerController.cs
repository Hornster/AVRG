using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _maxVelocity = 500f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// Rotates moving vector so that it's Z axis is always aiming where the player's looking.
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    Vector3 RotateMoveVector(Vector3 move)
    {
        return -(transform.rotation * move);
    }

    Vector3 ReadAxes()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");

        //Vertical axis comes through the player's eyes and makes them move forward or backwards
        return new Vector3(horizontalAxis, 0, verticalAxis);
    }
    void MovePlayer(Vector3 axes)
    {
        float timeDelta = Time.deltaTime;
        Vector3 currentVelocity = axes * _maxVelocity;
        currentVelocity *= timeDelta;
        currentVelocity = RotateMoveVector(currentVelocity);

        gameObject.transform.Translate(currentVelocity);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 axes = ReadAxes();
        MovePlayer(axes);
    }
}
