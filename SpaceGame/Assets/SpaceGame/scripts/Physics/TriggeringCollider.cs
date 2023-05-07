using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class TriggeringCollider : MonoBehaviour
    {
        public UnityEvent StartedTriggering;
        public UnityEvent StoppedTriggering;
    }
}
