using System;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpaceGame
{
    public class Health : MonoBehaviour
    {

        [ShowInInspector]
        private float _healthValue = 100f;

        [Required] public Slider HealthSlider;

        public UnityEvent HealthDecreased = new();
        public UnityEvent HealthIncreased = new();

        [Tooltip("Emmitted when health is at 0")]
        public UnityEvent HealthDepleted = new();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Start() => HealthSlider.SetValueWithoutNotify(_healthValue);

        [Button]
        public void UpdateHealth(float amount)
        {
            _healthValue = Math.Max(_healthValue + amount, 0);
            HealthSlider.SetValueWithoutNotify(_healthValue);
            (amount > 0f ? HealthIncreased : HealthDecreased).Invoke();
            if (_healthValue == 0)
                HealthDepleted.Invoke();
        }
    }
}
