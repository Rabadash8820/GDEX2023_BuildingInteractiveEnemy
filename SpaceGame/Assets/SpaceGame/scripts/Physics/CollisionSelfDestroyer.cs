using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class CollisionSelfDestroyer : MonoBehaviour
    {
        public UnityEvent Destroyed;

        void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(collision.otherRigidbody.gameObject);
            Destroyed.Invoke();
        }
    }
}
