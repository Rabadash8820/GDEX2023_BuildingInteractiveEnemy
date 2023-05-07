using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SpaceGame
{
    public class InputPushPuller : MonoBehaviour
    {
        private InputAction _pushPullInput;
        private bool _inProgress;

        [Required] public Transform LineOfSightTransform;
        [Required] public CapsuleCollider2D TriggerCapsuleCollider;
        [Required] public CollidingCollidersCollection CollidingCollidersCollection;

        private const string GRP_PHYSICS = "Physics";

        [FoldoutGroup(GRP_PHYSICS)]
        [MinValue(0d)]
        public int MaxRigidbodiesAffected = 10;

        [FoldoutGroup(GRP_PHYSICS)]
        public AnimationCurve ForceMultiplierWithDistance = AnimationCurve.Linear(0f, 10f, 1f, 0f);

        [FoldoutGroup(GRP_PHYSICS)]
        [MinValue(0d)]
        public int MaxSpeedAlongAxis = 3;

        public UnityEvent Started;
        public UnityEvent Performed;

        public float InputValue { get; private set; }

        private void Awake()
        {
            _pushPullInput = new MainInput().Player.PushPull;
        }

        private void FixedUpdate()
        {
            InputValue = _pushPullInput.ReadValue<float>();
            if (InputValue != 0f && !_inProgress)
            {
                _inProgress = true;
                Started.Invoke();
            }
            else if (InputValue == 0f && _inProgress)
            {
                _inProgress = false;
                Performed.Invoke();
            }

            if (InputValue == 0f)
                return;

            foreach (var collider in CollidingCollidersCollection.Colliders)
            {
                Rigidbody2D rb = collider.attachedRigidbody;
                if (rb == null)
                    continue;

                float pushPullRange = TriggerCapsuleCollider.size.x;
                Vector2 rbOffset = rb.position - (Vector2)LineOfSightTransform.position;
                Vector3 posAlongAxis = Vector3.Project(rbOffset, LineOfSightTransform.right);
                float posDistanceRatio = posAlongAxis.magnitude / pushPullRange;
                rb.AddForce(ForceMultiplierWithDistance.Evaluate(posDistanceRatio) * InputValue * rbOffset);
            }
        }

        public void OnEnable() => _pushPullInput.Enable();
        public void OnDisable() => _pushPullInput.Disable();
    }
}
