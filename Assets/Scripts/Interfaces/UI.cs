using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private Slider _healthbar;
        [SerializeField] private Player _player;

        private void OnDisable()
        {
            _player.HealChangeUnsubscribe(UpdateHealthBar);
        }

        private void Start()
        {
            _player.HealChangeSubscribe(UpdateHealthBar);
            _healthbar.maxValue = _player.MaxHealtValue;
            _healthbar.value = _player.HealthValue;
        }

        private void UpdateHealthBar(float health)
        {
            _healthbar.value = health;
        }
    }
}