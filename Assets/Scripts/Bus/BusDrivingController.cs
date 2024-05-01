using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDrivingController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _FLWheel, _FRWheel, _BLWheel, _BRWheel;
    [SerializeField] private WheelCollider _FLWheelCol, _FRWheelCol, _BLWheelCol, _BRWheelCol;

    private Rigidbody rb;
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
        //UpdateWheel();
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


    private void UpdateWheel()
    {
        UpdatePos(_FLWheelCol, _FLWheel);
        UpdatePos(_FRWheelCol, _FRWheel);
        UpdatePos(_BLWheelCol, _BLWheel);
        UpdatePos(_BRWheelCol, _BRWheel);
    }
    private void UpdatePos(WheelCollider col, MeshRenderer mesh)
    {
        Quaternion quaternion;
        Vector3 pos;
        col.GetWorldPose(out pos, out quaternion);
        
        mesh.transform.position = pos;
        mesh.transform.rotation = quaternion;

        mesh.transform.localScale = col.transform.localScale;
    }
}
