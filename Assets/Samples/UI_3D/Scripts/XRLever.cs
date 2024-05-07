using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine.XR.Content.Interaction
{
    public enum LeverState
    {
        Forward,
        Neutral,
        Reverse
    }

    public class XRLever : XRBaseInteractable
    {
        const float k_LeverDeadZone = 0.1f;

        [SerializeField] Transform m_Handle = null;
        [SerializeField] LeverState m_State = LeverState.Neutral;
        [SerializeField] bool m_LockToState;
        [SerializeField] float m_MaxAngle = 90.0f;
        [SerializeField] float m_MinAngle = -90.0f;
        [SerializeField] UnityEvent m_OnStateChange = new UnityEvent();

        IXRSelectInteractor m_Interactor;

        public Transform handle
        {
            get => m_Handle;
            set => m_Handle = value;
        }

        public LeverState state
        {
            get => m_State;
            set => SetState(value, true);
        }

        public bool lockToState
        {
            get => m_LockToState;
            set => m_LockToState = value;
        }

        public float maxAngle
        {
            get => m_MaxAngle;
            set => m_MaxAngle = value;
        }

        public float minAngle
        {
            get => m_MinAngle;
            set => m_MinAngle = value;
        }

        public UnityEvent onStateChange => m_OnStateChange;

        void Start()
        {
            SetState(m_State, true);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        void StartGrab(SelectEnterEventArgs args)
        {
            m_Interactor = args.interactorObject;
        }

        void EndGrab(SelectExitEventArgs args)
        {
            SetState(m_State, true);
            m_Interactor = null;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (isSelected)
                {
                    UpdateState();
                }
            }
        }

        Vector3 GetLookDirection()
        {
            Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
            direction = transform.InverseTransformDirection(direction);
            direction.x = 0;

            return direction.normalized;
        }

        void UpdateState()
        {
            var lookDirection = GetLookDirection();
            var lookAngle = Mathf.Atan2(lookDirection.z, lookDirection.y) * Mathf.Rad2Deg;

            if (m_MinAngle < m_MaxAngle)
                lookAngle = Mathf.Clamp(lookAngle, m_MinAngle, m_MaxAngle);
            else
                lookAngle = Mathf.Clamp(lookAngle, m_MaxAngle, m_MinAngle);

            var maxAngleDistance = Mathf.Abs(m_MaxAngle - lookAngle);
            var minAngleDistance = Mathf.Abs(m_MinAngle - lookAngle);

            var newState = LeverState.Neutral;

            if (maxAngleDistance < minAngleDistance * (1.0f - k_LeverDeadZone))
                newState = LeverState.Forward;
            else if (minAngleDistance < maxAngleDistance * (1.0f - k_LeverDeadZone))
                newState = LeverState.Reverse;

            SetHandleAngle(newState);

            SetState(newState);
        }

        void SetState(LeverState newState, bool forceRotation = false)
        {
            if (m_State == newState)
            {
                if (forceRotation)
                    SetHandleAngle(newState);

                return;
            }

            m_State = newState;

            m_OnStateChange.Invoke();

            if (!isSelected && (m_LockToState || forceRotation))
                SetHandleAngle(newState);
        }

        void SetHandleAngle(LeverState state)
        {
            float angle = 0f;
            switch (state)
            {
                case LeverState.Forward:
                    angle = m_MaxAngle;
                    break;
                case LeverState.Neutral:
                    angle = (m_MaxAngle + m_MinAngle) / 2f;
                    break;
                case LeverState.Reverse:
                    angle = m_MinAngle;
                    break;
            }

            if (m_Handle != null)
                m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0.0f);
        }

        void OnDrawGizmosSelected()
        {
            var angleStartPoint = transform.position;

            if (m_Handle != null)
                angleStartPoint = m_Handle.position;

            const float k_AngleLength = 0.25f;

            var angleMaxPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MaxAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;
            var angleMinPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MinAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(angleStartPoint, angleMaxPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(angleStartPoint, angleMinPoint);
        }

        void OnValidate()
        {
            SetHandleAngle(m_State);
        }
    }
}
