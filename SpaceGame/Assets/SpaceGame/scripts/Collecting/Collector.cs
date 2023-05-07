using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    [RequireComponent(typeof(Collider2D))]
    public class Collector : MonoBehaviour
    {
        public UnityEvent Collected = new();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.attachedRigidbody || !collider.attachedRigidbody.TryGetComponent(out Collectible collectible))
                return;

            bool wasCollected = collectible.Collect();
            if (wasCollected)
                Collected.Invoke();
        }
    }
}
