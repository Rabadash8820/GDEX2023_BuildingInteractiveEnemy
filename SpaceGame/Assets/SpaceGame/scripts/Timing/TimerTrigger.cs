using Sirenix.OdinInspector;
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
