using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class MonsterRegionEventArgs : EventArgs
    {
        public MonsterRegionEventArgs(PolygonCollider2D regionTriggerCollider, Collider2D triggeringCollider)
        {
            RegionTriggerCollider = regionTriggerCollider;
            TriggeringCollider = triggeringCollider;
        }
        public PolygonCollider2D RegionTriggerCollider;
        public Collider2D TriggeringCollider;
    }

    public class MonsterRegionListener : MonoBehaviour
    {
        [Required] public PolygonCollider2D RegionTriggerCollider;

        public event EventHandler<MonsterRegionEventArgs> TriggerEnterred;
        public event EventHandler<MonsterRegionEventArgs> TriggerExited;

        private void OnTriggerEnter2D(Collider2D collider) =>
            TriggerEnterred?.Invoke(sender: this, new(RegionTriggerCollider, collider));
        private void OnTriggerExit2D(Collider2D collider) =>
            TriggerExited?.Invoke(sender: this, new(RegionTriggerCollider, collider));
    }
}
