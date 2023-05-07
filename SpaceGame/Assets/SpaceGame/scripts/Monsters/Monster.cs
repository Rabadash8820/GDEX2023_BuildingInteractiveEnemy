using System;
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

        public bool DrawGizmos = true;

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
