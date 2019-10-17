using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = System.Random;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    float rotateAnglePerFrame = 1;
    System.Random randomGenerator = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float GenerateFloat()
    {
        return (float) randomGenerator.NextDouble();
    }
    // Update is called once per frame
    void Update()
    {
        
        var axis = new Vector3(GenerateFloat(), GenerateFloat(), GenerateFloat());
        
        transform.Rotate(axis, rotateAnglePerFrame);
    }
}
