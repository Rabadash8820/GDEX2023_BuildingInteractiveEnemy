using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceGame
{
    public class TreasureCollectible : Collectible
    {
        [TextArea]
        public string Description = "";

        public Sprite Sprite;

        [Required]
        public RigidbodyFollower RigidbodyFollower;
    }
}
