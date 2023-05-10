// ----------------------------------------------------------------------------
// Inspired by Unite 2017 talk: "Game Architecture with Scriptable Objects"
// See: https://github.com/roboryantron/Unite2017/blob/master/Assets/Code/Events/GameEvent.cs
// ----------------------------------------------------------------------------

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame
{
    [CreateAssetMenu(menuName = nameof(SpaceGame) + "/" + nameof(GameEvent), fileName = "event-something-happened")]
    public class GameEvent : ScriptableObject
    {
        public event EventHandler Invoked;

        public UnityEvent BaseActions = new UnityEvent();

        [Button]
        public void Invoke()
        {
            BaseActions.Invoke();
            Invoked?.Invoke(this, EventArgs.Empty);
        }
    }
}
