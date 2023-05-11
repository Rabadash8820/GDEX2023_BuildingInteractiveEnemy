using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class CollisionSelfDestroyer : MonoBehaviour
    {
        public UnityEvent Destroyed = new();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(collision.otherRigidbody.gameObject);
            Destroyed.Invoke();
        }
    }
}
