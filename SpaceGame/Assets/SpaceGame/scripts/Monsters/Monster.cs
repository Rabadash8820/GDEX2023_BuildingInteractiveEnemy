using System;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceGame
{
    public class Monster : MonoBehaviour
    {
        [Required] public Rigidbody2DTransformFollower Rigidbody2DTransformFollower;

        [ShowInInspector, ReadOnly, NonSerialized]
        public Transform SpawnPoint;

        [ShowInInspector, ReadOnly, NonSerialized]
        public Transform GuardPoint;

        [Required] public LookAtTriggerCollider LaserTriggerCollider;

        public bool DrawGizmos = true;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnDrawGizmos()
        {
            if (!DrawGizmos)
                return;

            Gizmos.color = Color.red;
            if (Rigidbody2DTransformFollower.TransformToFollow)
                Gizmos.DrawLine(Rigidbody2DTransformFollower.RigidbodyToMove.position, Rigidbody2DTransformFollower.TransformToFollow.position);
        }
    }
}
