// ----------------------------------------------------------------------------
// Inspired by Unite 2017 talk: "Game Architecture with Scriptable Objects"
// See: https://github.com/roboryantron/Unite2017/blob/master/Assets/Code/Events/GameEventListener.cs
// ----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace SpaceGame
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event to which we listen")]
        public GameEventReference EventListener;

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        public void OnEnable() => EventListener.OnEnable();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        public void OnDisable() => EventListener.OnDisable();
    }
}
