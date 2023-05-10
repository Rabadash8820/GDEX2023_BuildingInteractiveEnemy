// ----------------------------------------------------------------------------
// Inspired by Unite 2017 talk: "Game Architecture with Scriptable Objects"
// See: https://github.com/roboryantron/Unite2017/blob/master/Assets/Code/Events/GameEventListener.cs
// ----------------------------------------------------------------------------

using Sirenix.OdinInspector;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    [Serializable]
    public class GameEventReference
    {
        [Required, Tooltip("Event to listen to")]
        public GameEvent Event;

        [Tooltip("Methods to call when " + nameof(Event) + " is raised")]
        public UnityEvent Actions = new UnityEvent();

        public void OnEnable() => Event!.Invoked += doInvoke;
        public void OnDisable() => Event!.Invoked -= doInvoke;

        private void doInvoke(object sender, EventArgs e) => Actions.Invoke();

        [Button, ShowInInspector]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Invoked by Odin Inspector button in Unity Editor")]
        private void invoke() => Actions.Invoke();
    }
}
