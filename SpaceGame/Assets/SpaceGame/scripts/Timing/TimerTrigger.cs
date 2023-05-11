using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    public class TimerTrigger : MonoBehaviour
    {
        private float _tElapsed;
        private bool _complete = true;

        [MinValue(0d)]
        public float Duration;
        public bool StartWithComponentStart;
        public UnityEvent Complete = new();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Start()
        {
            if (StartWithComponentStart)
                RestartTimer();
        }

        public void RestartTimer()
        {
            _tElapsed = 0f;
            _complete = false;
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Update()
        {
            if (!_complete && (_tElapsed += Time.deltaTime) >= Duration)
            {
                _complete = true;
                Complete.Invoke();
            }
        }
    }
}
