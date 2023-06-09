using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

namespace SpaceGame
{
    public class TreasureCollector : CollectorBase<TreasureCollectible>
    {
        [Required] public Transform PlayerTransform;
        [RequiredIn(PrefabKind.InstanceInScene)] public TMP_Text TxtCollectedTreasure;
        public Vector2 CollectedPlayerOffset = new(-2f, 0f);

        public TreasureCollectible CollectedTreasure { get; private set; }

        protected override bool TryCollect(TreasureCollectible treasure)
        {
            if (CollectedTreasure != null) {
                Debug.Log($"{GetType().Name} has already collected treasure '{CollectedTreasure.Description}'; cannot collect anything else");
                return false;
            }

            CollectedTreasure = treasure;
            treasure.Rigidbody2DTransformFollower.TransformToFollow = PlayerTransform;

            return true;
        }

        [Button]
        public void Drop()
        {
            if (CollectedTreasure != null)
                Drop(CollectedTreasure);
        }

        public override void Drop(TreasureCollectible treasure)
        {
            if (treasure == null)
                throw new ArgumentNullException(nameof(treasure));
            if (treasure != CollectedTreasure) {
                Debug.Log($"{GetType().Name} '{name}' cannot drop treasure '{treasure.Description}', as it was never collected");
                return;
            }

            Debug.Log($"{GetType().Name} '{name}' dropped treasure '{CollectedTreasure.Description}'");
            CollectedTreasure.Rigidbody2DTransformFollower.TransformToFollow = null;
            CollectedTreasure = null;
            treasure.Dropped.Invoke();
        }
    }
}
