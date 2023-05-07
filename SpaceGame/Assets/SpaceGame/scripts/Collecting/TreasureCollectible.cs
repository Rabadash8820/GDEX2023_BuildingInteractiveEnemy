using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class TreasureCollectible : Collectible
    {
        [Required] public TreasureCollectionManager TreasureCollectionManager;

        [TextArea]
        public string Name = "";
        public Sprite Sprite;
        [Required] public RigidbodyFollower RigidbodyFollower;

        public UnityEvent Collected = new();

        public override bool Collect()
        {
            bool wasCollected = TreasureCollectionManager.CollectTreasure(this);
            if (wasCollected)
                Collected.Invoke();

            return wasCollected;
        }
    }
}
