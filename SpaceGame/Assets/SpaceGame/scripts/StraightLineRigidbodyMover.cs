using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace SpaceGame
{
    public class StraightLineRigidbodyMover : MonoBehaviour
    {
        [Required] public Rigidbody2D RigidbodyToMove;
        [MinValue(0d)] public float Speed;
        public Vector2 LocalDirection = Vector2.right;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void FixedUpdate()
        {
            var dir = (Vector2)RigidbodyToMove.transform.TransformDirection(LocalDirection);
            RigidbodyToMove.MovePosition(RigidbodyToMove.position + Speed * Time.fixedDeltaTime * dir);
        }
    }
}
