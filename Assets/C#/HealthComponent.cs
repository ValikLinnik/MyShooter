using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace MyNamespace
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField, Range(0, 100)]
        private float _currentHealth;
    
        public float CurrentHealthValue
        {
            get
            {
                return _currentHealth;
            }

            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealthValue);
            }
        }

        public float MaxHealthValue
        {
            get
            {
                return _maxHealthValue;
            }

        }

        public event Action<float, float> OnDamageGet;

        private void OnDamageGetHandler(float damage, float currentHealth)
        {
            if(OnDamageGet != null) OnDamageGet(damage, currentHealth);
        }

        private float _maxHealthValue;

        private void Start()
        {
            _maxHealthValue = _currentHealth;
        }

        public void TakeDamege(float damage)
        {
            _currentHealth -= damage;
            OnDamageGetHandler(damage, _currentHealth);
        }
    }
}
