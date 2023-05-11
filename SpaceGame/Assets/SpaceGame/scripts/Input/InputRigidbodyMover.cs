using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SpaceGame
{
    public class InputRigidbodyMover : MonoBehaviour
    {
        private InputAction _moveInput;
        private bool _inProgress;
        private Vector2 _targetVelocity = Vector2.zero;

        [Required] public Rigidbody2D RigidbodyToMove;

        [MinValue(0d)]
        public float TargetSpeed = 10f;

        [MinValue(0d)]
        public float MaxAcceleration = 100f;

        public UnityEvent Started = new();
        public UnityEvent Performed = new();

        public float SpeedSign { get; private set; }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Awake() => _moveInput = new MainInput().Player.Move;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void FixedUpdate()
        {
            SpeedSign = _moveInput.inProgress ? _moveInput.ReadValue<float>() : 0f;
            if (SpeedSign != 0f && !_inProgress)
            {
                _inProgress = true;
                Started.Invoke();
            }
            else if (SpeedSign == 0f && _inProgress)
            {
                _inProgress = false;
                Performed.Invoke();
            }

            _targetVelocity = TargetSpeed * SpeedSign * RigidbodyToMove.transform.right;
            Vector2 accelNeeded = _targetVelocity - RigidbodyToMove.velocity;
            float accelMag = accelNeeded.magnitude;
            if (accelMag == 0)
                return;

            ForceMode2D forceMode = _moveInput.inProgress ? ForceMode2D.Impulse : ForceMode2D.Force;
            Vector2 accelDir = accelNeeded / accelMag;
            Vector2 accelToDo = Mathf.Clamp(accelMag, 0f, MaxAcceleration) * accelDir;
            RigidbodyToMove.AddForce(accelToDo, forceMode);
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnEnable() => _moveInput.Enable();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnDisable() => _moveInput.Disable();
    }
}
