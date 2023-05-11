using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class RepeaterTrigger : MonoBehaviour
    {
        private bool _running = true;

        [MinValue(0d)] public float RepeatDuration;
        public bool StopAfterMaxRepeats = false;
        [ShowIf(nameof(StopAfterMaxRepeats)), MinValue(1d)] public int MaxRepeats = 1;
        public bool StartWithComponentStart;

        [ShowInInspector] private float _tElapsed;
        [ShowInInspector] private int _repeatCount;

        public UnityEvent RepeatTriggered = new();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Start()
        {
            if (StartWithComponentStart)
                Restart();
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnEnable() => Restart();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnDisable() => stop();

        public void Restart()
        {
            _repeatCount = 0;
            _tElapsed = 0f;
            _running = true;
        }

        public void Stop() => stop();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Update()
        {
            if (_running && (_tElapsed += Time.deltaTime) >= RepeatDuration)
            {
                RepeatTriggered.Invoke();
                _tElapsed -= RepeatDuration;
                ++_repeatCount;
                if (StopAfterMaxRepeats && _repeatCount == MaxRepeats)
                    stop();
            }
        }

        private void stop()
        {
            _tElapsed = 0f;
            _running = false;
        }
    }
}
