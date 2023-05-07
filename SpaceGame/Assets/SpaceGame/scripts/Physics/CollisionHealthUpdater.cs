using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class CollisionHealthUpdater : MonoBehaviour
    {
        public float UpdateHealthMaxAmount = -10f;
        [Required] public Health Health;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name != "space-station-collider")
            {
                var damageRatio = collision.relativeVelocity.magnitude / 10;
                var damageAmount = Math.Max(UpdateHealthMaxAmount, UpdateHealthMaxAmount * damageRatio);
                Health.UpdateHealth(damageAmount);
            }
        }
    }
}
