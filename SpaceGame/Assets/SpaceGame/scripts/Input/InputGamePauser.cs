using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SpaceGame
{
    public class InputGamePauser : MonoBehaviour
    {
        private InputAction _pauseInput;

        public bool IsPaused { get; private set; }

        public UnityEvent Paused = new();
        public UnityEvent Resumed = new();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Awake()
        {
            _pauseInput = new MainInput().Player.Pause;
            _pauseInput.started += ctx => togglePausedState(!IsPaused);
        }

        public void PauseGame() => togglePausedState(true);
        public void ResumeGame() => togglePausedState(false);

        private void togglePausedState(bool isPaused)
        {
            if (isPaused == IsPaused)
                return;

            (isPaused ? Paused : Resumed).Invoke();
            IsPaused = isPaused;
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnEnable() => _pauseInput.Enable();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnDisable() => _pauseInput.Disable();
    }
}
