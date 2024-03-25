using System;
using UnityEngine;

namespace Game
{
    public class Health
    {
        private const float Correction = 0.01f;
        private const float MinHealthPoint = 0f;

        private float _maxHealthPoint;
        private float _currentHealthPoint;
        private float _onePercent;

        public event Action<float> OnChange;

        public Health(float healthPoint)
        {
            _currentHealthPoint = healthPoint;
            _maxHealthPoint = healthPoint;
            _onePercent = _maxHealthPoint * Correction;
        }

        public Health(float currentHealthPoint, float maxHealthPoint)
        {
            _maxHealthPoint = maxHealthPoint;
            Change(currentHealthPoint);
        }

        public float Value { get { return _currentHealthPoint; } }
        public float PercentValue { get { return GetProcentValue(); } }
        public float MaxValue { get { return _maxHealthPoint; } }

        public void Increase(float healthPoint)
        {
            if (healthPoint > 0)
                Change(_currentHealthPoint + healthPoint);
        }

        public void Decrease(float healthPoint)
        {
            if (healthPoint > 0)
                Change(_currentHealthPoint - healthPoint);
        }

        public void ChangeMax(float maxHealthPoint)
        {
            if (maxHealthPoint > 0)
            {
                _maxHealthPoint = maxHealthPoint;
                _onePercent = _maxHealthPoint * Correction;
                Change(_currentHealthPoint);
            }
        }

        private void Change(float currentHealthPoint)
        {
            _currentHealthPoint = Mathf.Clamp(currentHealthPoint, MinHealthPoint, _maxHealthPoint);
            OnChange?.Invoke(GetProcentValue());
        }

        private float GetProcentValue()
        {
            return _currentHealthPoint / _onePercent * Correction;
        }
    }
}