using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private Slider _healthbar;

        private float _health;

        public void Healing(float healthPoints)
        {
            if (healthPoints > 0)
            {
                _health += healthPoints;

                if (_health > 100)
                    _health = 100;
            }
        }

        private void Start()
        {
            _health = 100;
            _healthbar.value = _health;
        }

        private void LateUpdate()
        {
            _health -= 0.1f;
            _healthbar.value = _health;
        }
    }
}