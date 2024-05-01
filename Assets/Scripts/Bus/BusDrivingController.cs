using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDrivingController : MonoBehaviour
{
    [SerializeField] private Transform _FLWheel, _FRWheel, _BLWheel, _BRWheel;
    [SerializeField] private WheelCollider _FLWheelCol, _FRWheelCol, _BLWheelCol, _BRWheelCol;

    [SerializeField] private Rigidbody rb;

    private float moveInput;
    private float steeringInput;

    [SerializeField] private float motorPower;
    [SerializeField] private float steeringPower;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckInputs();
        AppplyPower();
        ApplySteering();
    }

    private void CheckInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
    }

    private void AppplyPower()
    {
        _BLWheelCol.motorTorque = motorPower * moveInput;
        _BRWheelCol.motorTorque = motorPower * moveInput;
    }

    private void ApplySteering()
    {
        _FLWheelCol.steerAngle = steeringInput * steeringPower;
        _FRWheelCol.steerAngle = steeringInput * steeringPower;
    }
}
