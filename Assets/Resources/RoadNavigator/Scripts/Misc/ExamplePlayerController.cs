using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Content.Interaction;

namespace InsaneSystems.RoadNavigator.Misc
{
    public class ExamplePlayerController : MonoBehaviour
    {
        [SerializeField] private XRLever _lever;
        [SerializeField] private XRKnob _knob;
        [SerializeField] private TMP_Text speedText;

        private NavMeshAgent navAgent;
        private float moveInput;
        private float steeringInput;

        public float motorPower = 5f; // 기본 모터 파워
        [SerializeField] private float maxSpeed = 10f; // 최대 속도
        [SerializeField] private float acceleration = 2f; // 가속도
        [SerializeField] private float steeringPower = 30f; // 기본 스티어링 파워

        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.speed = 0f; // 초기 속도 0

            if (_lever == null)
            {
                Debug.LogWarning("XRLever reference not set.");
            }
            else
            {
                _lever.onStateChange.AddListener(OnLeverStateChange);
            }

            if (_knob == null)
            {
                Debug.LogWarning("XRKnob reference not set.");
            }
            else
            {
                _knob.onValueChange.AddListener(OnKnobValueChange);
            }
        }

        void Update()
        {
            CheckInputs();
            ApplyMovement();
            UpdateSpeedText();
        }

        private void CheckInputs()
        {
            if (_lever == null || _knob == null)
            {
                return;
            }

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

        private void ApplyMovement()
        {
            if (moveInput != 0f)
            {
                float targetSpeed = motorPower * moveInput * maxSpeed;
                navAgent.speed = Mathf.MoveTowards(navAgent.speed, targetSpeed, acceleration * Time.deltaTime);

                float steering = steeringPower * steeringInput;

                // 이동 방향 계산
                Vector3 moveDirection = transform.forward * navAgent.speed * Time.deltaTime;
                Vector3 steeringDirection = transform.right * steering * Time.deltaTime;

                // 목적지 설정
                navAgent.destination = transform.position + moveDirection + steeringDirection;
            }
            else
            {
                // 속도가 점차적으로 감소하도록 설정
                navAgent.speed = Mathf.MoveTowards(navAgent.speed, 0f, acceleration * Time.deltaTime);
            }
        }

        private void UpdateSpeedText()
        {
            if (speedText != null)
            {
                speedText.text = "Speed: " + (navAgent.velocity.magnitude * 3.6f).ToString("F2") + " km/h"; // 속도를 km/h로 표시
            }
        }

        private void OnLeverStateChange()
        {
            CheckInputs();
        }

        private void OnKnobValueChange(float newValue)
        {
            // 레버의 상태가 중립이 아닐 때만 입력을 확인하도록 합니다.
            if (_lever.state != LeverState.Neutral)
            {
                CheckInputs();
            }
        }
    }
}
