using System;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour, IDamageble
    {
        private Health _health;
        private float _physicalResistance;
        private float _armor;

        private void Start()
        {
            _health = new Health();
            _physicalResistance = 0.1f;
            _armor = 0.05f;
        }

        public float MaxHealtValue { get { return _health.MaxValue; } }
        public float HealthValue { get { return _health.Value; } }

        public void HealChangeSubscribe(Action<float> function)
        {
            _health.HealthChanged += function;
        }

        public void HealChangeUnsubscribe(Action<float> function)
        {
            _health.HealthChanged -= function;
        }

        public void TakeDamage(float damage)
        {
            damage -= damage * _physicalResistance * _armor;
            damage -= damage * _armor;
            _health.Decrease(damage);
        }

        public void Healing(float healthPoints)
        {
            _health.Increase(healthPoints);
        }

        private void LateUpdate()                                   ////
        {
            TakeDamage(0.1f);
        }
    }
}