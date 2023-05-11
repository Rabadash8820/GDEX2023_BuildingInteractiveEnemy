using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class TriggeringCollider : MonoBehaviour
    {
        public UnityEvent StartedTriggering = new();
        public UnityEvent StoppedTriggering = new();
    }
}
