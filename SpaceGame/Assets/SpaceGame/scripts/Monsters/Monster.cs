using System;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class Monster : MonoBehaviour
    {
        [Required] public RigidbodyFollower RigidbodyFollower;

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
            if (RigidbodyFollower.TransformToFollow)
                Gizmos.DrawLine(RigidbodyFollower.RigidbodyToMove.position, RigidbodyFollower.TransformToFollow.position);
        }
    }
}
