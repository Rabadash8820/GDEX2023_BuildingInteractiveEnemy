using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class Rigidbody2DLookAt : MonoBehaviour
    {
        [Required] public Transform LookAtTransform;
        [Required] public Rigidbody2D LookerRigidbody;

        private void FixedUpdate()
        {
            Vector2 vectorBetween = (Vector2)LookAtTransform.position - LookerRigidbody.position;
            float angle = Vector2.Angle(LookerRigidbody.transform.right, vectorBetween);
            LookerRigidbody.MoveRotation(90f - angle);
        }
    }
}
