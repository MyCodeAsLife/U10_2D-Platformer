using System;

namespace Game
{
    public class Health
    {
        private float _currentHealthPoint;
        private float _maxHealthPoint;

        public event Action<float> HealthChanged;
        public event Action<float> MaxHealthChanged;

        public Health()
        {
            _currentHealthPoint = 100;
            _maxHealthPoint = 100;
        }

        public Health(float healthPoint)
        {
            _currentHealthPoint = healthPoint;
            _maxHealthPoint = healthPoint;
        }

        public Health(float currentHealthPoint, float maxHealthPoint)
        {
            _maxHealthPoint = maxHealthPoint;

            if (currentHealthPoint < maxHealthPoint && currentHealthPoint > 0)
                _currentHealthPoint = currentHealthPoint;
            else
                _currentHealthPoint = maxHealthPoint;
        }

        public float Value { get { return _currentHealthPoint; } }
        public float MaxValue { get { return _maxHealthPoint; } }

        public void Increase(float healthPoint)
        {
            if (healthPoint > 0)
            {
                _currentHealthPoint += healthPoint;

                if (_currentHealthPoint > _maxHealthPoint)
                    _currentHealthPoint = _maxHealthPoint;

                HealthChanged?.Invoke(_currentHealthPoint);
            }
        }

        public void Decrease(float healthPoint)
        {
            if (healthPoint > 0)
            {
                _currentHealthPoint -= healthPoint;

                if (_currentHealthPoint <= 0)
                    _currentHealthPoint = 0;

                HealthChanged?.Invoke(_currentHealthPoint);
            }
        }

        public void ChangeMaxHP(float maxHealthPoint)
        {
            if (maxHealthPoint > 0)
            {
                _maxHealthPoint = maxHealthPoint;

                if (_currentHealthPoint > _maxHealthPoint)
                    _currentHealthPoint = _maxHealthPoint;

                HealthChanged?.Invoke(_currentHealthPoint);
                MaxHealthChanged?.Invoke(_maxHealthPoint);
            }
        }
    }
}