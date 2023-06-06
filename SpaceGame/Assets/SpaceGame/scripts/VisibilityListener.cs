using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    [RequireComponent(typeof(Renderer))]
    public class VisibilityListener : MonoBehaviour
    {
        private bool _inRefactoryVisible;
        private bool _inRefactoryInvisible;
        private float _tElapsedVisible;
        private float _tElapsedInvisible;

        public float VisibleRefactoryPeriod = 3f;
        public float InvisibleRefactoryPeriod = 3f;

        public UnityEvent BecameVisible = new();
        public UnityEvent BecameInvisible = new();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Update()
        {
            float dt = Time.deltaTime;
            if (_inRefactoryVisible && (_tElapsedVisible += dt) >= VisibleRefactoryPeriod)
            {
                _inRefactoryVisible = false;
                _tElapsedVisible = 0f;
            }
            if (_inRefactoryInvisible && (_tElapsedInvisible += dt) >= InvisibleRefactoryPeriod)
            {
                _inRefactoryInvisible = false;
                _tElapsedInvisible = 0f;
            }
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnBecameVisible()
        {
            if (!_inRefactoryVisible)
                BecameVisible.Invoke();
            _inRefactoryVisible = true;
            _tElapsedVisible = 0f;
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnBecameInvisible()
        {
            if (!_inRefactoryInvisible)
                BecameInvisible.Invoke();
            _inRefactoryInvisible = true;
            _tElapsedInvisible = 0f;
        }
    }
}
