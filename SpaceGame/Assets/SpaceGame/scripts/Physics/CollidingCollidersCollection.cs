using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    [RequireComponent(typeof(Collider2D))]
    public class CollidingCollidersCollection : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        public int TriggeringColliderCount => Colliders.Count;

        public ICollection<Collider2D> Colliders = new HashSet<Collider2D>();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnTriggerEnter2D(Collider2D collider)
        {
            Colliders.Add(collider);
            if (collider.attachedRigidbody && collider.attachedRigidbody.TryGetComponent(out TriggeringCollider triggeringCollider))
                triggeringCollider.StartedTriggering.Invoke();
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnTriggerExit2D(Collider2D collider)
        {
            Colliders.Remove(collider);
            if (collider.attachedRigidbody && collider.attachedRigidbody.TryGetComponent(out TriggeringCollider triggeringCollider))
                triggeringCollider.StoppedTriggering.Invoke();
        }
    }
}
