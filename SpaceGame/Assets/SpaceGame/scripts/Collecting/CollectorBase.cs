using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class CollectorBase<T> : MonoBehaviour where T : Collectible
    {
        public UnityEvent Collected = new();
        public string Name = "collectible";

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.attachedRigidbody || !collider.attachedRigidbody.TryGetComponent(out T collectible))
                return;

            bool wasCollected = TryCollect(collectible);
            if (wasCollected) {
                Debug.Log($"{GetType().Name} '{nameof(name)}' collected {collectible.GetType().Name} '{collectible.name}'");
                collectible.Collected.Invoke();
                Collected.Invoke();
            }
        }

        protected abstract bool TryCollect(T collectible);
        public abstract void Drop(T collectible);
    }
}
