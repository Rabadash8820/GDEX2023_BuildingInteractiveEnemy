using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public abstract class Collectible : MonoBehaviour
    {
        public UnityEvent Collected = new();
        public UnityEvent Dropped = new();
    }
}
