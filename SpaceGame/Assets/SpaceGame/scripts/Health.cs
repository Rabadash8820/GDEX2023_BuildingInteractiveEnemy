using System;
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

        public UnityEvent HealthDecreased;
        public UnityEvent HealthIncreased;

        [Tooltip("Emmitted when health is at 0")]
        public UnityEvent HealthDepleted;

        private void Start()
        {
            HealthSlider.SetValueWithoutNotify(_healthValue);
        }

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
