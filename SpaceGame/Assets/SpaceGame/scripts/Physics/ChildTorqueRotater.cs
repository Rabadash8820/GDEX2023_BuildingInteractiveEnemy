using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;
using U = UnityEngine;

namespace SpaceGame
{
    public class ChildTorqueRotater : MonoBehaviour
    {
        [MinMaxSlider(0f, 100f, showFields: true)]
        public Vector2 MassMultipleTorqueRange = new Vector2(0.5f, 1f);

        [MinMaxSlider(0f, 36000f, showFields: true)]
        public Vector2 KinematicAngularSpeedRange = new Vector2(10f, 90f);

        public bool RotateOnStart;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Start()
        {
            if (RotateOnStart)
                RandomlyRotateChildren();
        }

        [Button]
        public void RandomlyRotateChildren()
        {
            Rigidbody2D[] childRigidbodies = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rb in childRigidbodies)
            {
                switch (rb.bodyType)
                {
                    case RigidbodyType2D.Dynamic:
                        rb.AddTorque(getRandomTorque(MassMultipleTorqueRange) * rb.mass, ForceMode2D.Impulse);
                        break;

                    case RigidbodyType2D.Kinematic:
                        rb.angularVelocity = getRandomAngularVelocity(KinematicAngularSpeedRange);
                        break;

                    case RigidbodyType2D.Static:
                    default:
                        break;
                }
            }
        }

        [Button, Conditional("UNITY_EDITOR")]
        public void StopRotation()
        {
            Rigidbody2D[] childRigidbodies = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rb in childRigidbodies)
            {
                rb.angularVelocity = 0f;
            }
        }

        private static float getRandomTorque(Vector2 torqueRange)
        {
            float mag = (float)U.Random.Range(torqueRange.x, torqueRange.y);
            int toinCoss = U.Random.Range(minInclusive: 0, maxExclusive: 2);
            float dir = toinCoss == 0 ? 1 : -1;
            return dir * mag;
        }

        private static float getRandomAngularVelocity(Vector2 angularSpeedRange)
        {
            float angularSpeed = (float)U.Random.Range(angularSpeedRange.x, angularSpeedRange.y);
            int toinCoss = U.Random.Range(minInclusive: 0, maxExclusive: 2);
            float dir = toinCoss == 0 ? 1 : -1;
            return dir * angularSpeed;
        }
    }
}
