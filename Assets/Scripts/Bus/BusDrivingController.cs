using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class BusDrivingController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _FLWheel, _FRWheel, _BLWheel, _BRWheel;
    [SerializeField] private WheelCollider _FLWheelCol, _FRWheelCol, _BLWheelCol, _BRWheelCol;

    private Rigidbody rb;
    private float moveInput;
    private float steeringInput;

    [SerializeField] private float motorPower;
    [SerializeField] private float steeringPower;

    [SerializeField] private XRLever _lever;
    [SerializeField] private XRKnob _knob;
    
    [SerializeField] private TMP_Text speedText;

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
        DisplaySpeed();
    }

    private void CheckInputs()
    {
        switch (_lever.state)
        {
            case LeverState.Forward:
                moveInput = 1f;
                break;
            case LeverState.Neutral:
                moveInput = 0f;
                break;
            case LeverState.Reverse:
                moveInput = -1f;
                break;
        }
        
        steeringInput = _knob.value * 2f - 1f; 
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
    
    private void DisplaySpeed()
    {
        float speed = rb.velocity.magnitude * 3.6f;
        speedText.text = "Speed: " + speed.ToString("0.00") + " km/h";
    }
}
