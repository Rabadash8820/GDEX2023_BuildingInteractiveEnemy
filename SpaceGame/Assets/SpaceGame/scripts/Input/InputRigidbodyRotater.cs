using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SpaceGame
{
    public class InputRigidbodyRotater : MonoBehaviour
    {
        private InputAction _lookInput;
        private bool _inProgress;
        private float _targetTorque;

        [Required] public Rigidbody2D RigidbodyToRotate;

        [MinValue(0d)]
        public float TargetAngularSpeed = 360f;

        [MinValue(0d)]
        public float MaxTorque = 10f;

        public UnityEvent Started;
        public UnityEvent Performed;

        public float AngularSpeedSign { get; private set; }

        private void Awake()
        {
            _lookInput = new MainInput().Player.Look;
        }

        private void FixedUpdate()
        {
            AngularSpeedSign = _lookInput.inProgress ? _lookInput.ReadValue<float>() : 0f;
            if (AngularSpeedSign != 0f && !_inProgress)
            {
                _inProgress = true;
                Started.Invoke();
            }
            else if (AngularSpeedSign == 0f && _inProgress)
            {
                _inProgress = false;
                Performed.Invoke();
            }

            _targetTorque = AngularSpeedSign * TargetAngularSpeed;
            float torqueNeeded = _targetTorque - RigidbodyToRotate.angularVelocity;
            if (torqueNeeded == 0)
                return;

            ForceMode2D forceMode = _lookInput.inProgress ? ForceMode2D.Impulse : ForceMode2D.Force;
            float torqueToDo = Mathf.Clamp(torqueNeeded, -MaxTorque, MaxTorque);
            RigidbodyToRotate.AddTorque(torqueToDo, ForceMode2D.Force);
        }

        public void OnEnable() => _lookInput.Enable();
        public void OnDisable() => _lookInput.Disable();
    }
}
