using System;

namespace Game
{
    public class Health
    {
        private float _health;
        private float _maxHealth;

        public event Action<float> HealthChanged;

        public Health()
        {
            _maxHealth = 100;
            _health = _maxHealth;
        }

        public Health(float maxHealth)
        {
            _maxHealth = maxHealth;
            _health = _maxHealth;
        }

        public float Value { get { return _health; } }
        public float MaxValue { get { return _maxHealth; } }

        public void Increase(float points)
        {
            if (points > 0 && _health < _maxHealth)
            {
                _health += points;

                if (_health > _maxHealth)
                    _health = _maxHealth;

                HealthChanged?.Invoke(_health);
            }
        }

        public void Decrease(float points)
        {
            if (points > 0)
            {
                _health -= points;

                if (_health < 0)
                    _health = 0;

                HealthChanged?.Invoke(_health);
            }
        }
    }
}