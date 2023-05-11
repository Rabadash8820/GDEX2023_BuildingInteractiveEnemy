using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace SpaceGame
{
    public class ComponentLifecycleTrigger : MonoBehaviour
    {
        public GameEvent AwakeEvent;
        public GameEvent StartEvent;
        public GameEvent OnEnableEvent;
        public GameEvent OnDisableEvent;
        public GameEvent OnDestroyEvent;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Awake() => invoke(AwakeEvent);

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Start() => invoke(StartEvent);

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnEnable() => invoke(OnEnableEvent);

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnDisable() => invoke(OnDisableEvent);

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void OnDestroy() => invoke(OnDestroyEvent);

        private void invoke(GameEvent gameEvent)
        {
            if (gameEvent != null)
                gameEvent!.Invoke();
        }
    }
}
