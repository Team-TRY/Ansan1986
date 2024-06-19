using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;


    public class ExamplePlayerController : MonoBehaviour
    {
        [SerializeField] private XRLever _lever;
        [SerializeField] private XRKnob _knob;

        private float moveInput;
        private float steeringInput;

        public float motorPower = 5f; // 기본 모터 파워
        [SerializeField] private float maxSpeed = 10f; // 최대 속도
        [SerializeField] private float acceleration = 2f; // 가속도
        [SerializeField] private float steeringPower = 30f; // 기본 스티어링 파워
        [SerializeField] private GameObject audioSourceObject; 

        private AudioSource audioSource;
        
        private float currentSpeed = 0f;
        private Vector3 moveDirection;
        

        public float CurrentSpeed { get { return currentSpeed; } } // 현재 속도 프로퍼티

        void Start()
        {
            audioSource = audioSourceObject.GetComponent<AudioSource>();
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
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                    break;
                case LeverState.Neutral:
                    moveInput = 0f;
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                    break;
                case LeverState.Reverse:
                    moveInput = -1f;
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                    break;
            }

            steeringInput = _knob.value * 2f - 1f;
        }

        private void ApplyMovement()
        {
            if (moveInput != 0f)
            {
                float targetSpeed = motorPower * moveInput * maxSpeed;
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

                // 이동 방향 계산
                moveDirection = transform.forward * currentSpeed * Time.deltaTime;

                // 위치 업데이트
                transform.position += moveDirection;

                // 회전 처리
                float steering = steeringPower * steeringInput * Time.deltaTime;
                transform.Rotate(0, steering, 0);
            }
            else
            {
                // 속도가 점차적으로 감소하도록 설정
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
                transform.position += transform.forward * currentSpeed * Time.deltaTime;
            }
        }

        private void OnLeverStateChange()
        {
            CheckInputs();
        }

        private void OnKnobValueChange(float newValue)
        {
            // 레버의 상태가 중립이 아닐 때만 입력을 확인
            if (_lever.state != LeverState.Neutral)
            {
                CheckInputs();
            }
        }
    }

