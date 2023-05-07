using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class HelpUiManager : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] private bool _hasMoved;
        [ShowInInspector, ReadOnly] private bool _hasRotated;
        [ShowInInspector, ReadOnly] private bool _hasSeenPushPullTarget;
        [ShowInInspector, ReadOnly] private bool _hasPushed;
        [ShowInInspector, ReadOnly] private bool _hasPulled;
        private bool _pushPullUiShown;
        private bool _allUiShown;

        [Required] public InputRigidbodyMover InputRigidbodyMover;
        [Required] public InputRigidbodyRotater InputRigidbodyRotater;
        [Required] public InputPushPuller InputPushPuller;
        [Required] public CollidingCollidersCollection CollidingCollidersCollection;

        public UnityEvent HasMoved = new();
        public UnityEvent HasRotated = new();
        public UnityEvent PushPullUiNeeded = new();
        public UnityEvent HasPushPulled = new();

        private void Update()
        {
            if (_allUiShown)
                return;

            bool hasMoved = _hasMoved || InputRigidbodyMover.SpeedSign != 0f;
            bool hasRotated = _hasRotated || InputRigidbodyRotater.AngularSpeedSign != 0f;
            if (hasMoved && !_hasMoved) { _hasMoved = true; HasMoved.Invoke(); }
            if (hasRotated && !_hasRotated) { _hasRotated = true; HasRotated.Invoke(); }

            bool hasPushed = _hasPushed || InputPushPuller.InputValue > 0f;
            bool hasPulled = _hasPulled || InputPushPuller.InputValue < 0f;
            bool justPushed = hasPushed && !_hasPushed;
            bool justPulled = hasPulled && !_hasPulled;
            if (justPushed) _hasPushed = true;
            if (justPulled) _hasPulled = true;

            bool hasSeenPushPullTarget = _hasSeenPushPullTarget || CollidingCollidersCollection.Colliders.Count > 0;
            if (hasSeenPushPullTarget && !_hasSeenPushPullTarget)
            {
                _hasSeenPushPullTarget = true;
                if (_hasPushed && _hasPulled)
                {
                    _allUiShown = true;
                    return;
                }
                _pushPullUiShown = true;
                PushPullUiNeeded.Invoke();
            }

            if (_pushPullUiShown && (justPushed || justPulled))
            {
                _allUiShown = true;
                HasPushPulled.Invoke();
            }
        }
    }

}
