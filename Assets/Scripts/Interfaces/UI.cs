using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Character _character;

        private Coroutine _changesSmoothly;

        private void OnDisable()
        {
            _character.UnsubscribeHeathChanged(ChangeHealth);
            _character.UnsubscribeMaxHeathChanged(ChangeMaxHealth);
        }

        private void Start()                                                // Çהוסü
        {
            _character.SubscribeHeathChanged(ChangeHealth);
            _character.SubscribeMaxHeathChanged(ChangeMaxHealth);

            ChangeMaxHealth(_character.MaxHealth);
            ChangeHealth(_character.Health);
        }

        private void ChangeHealth(float healthPoint)
        {
            if (_changesSmoothly != null)
                StopCoroutine(_changesSmoothly);

            _changesSmoothly = StartCoroutine(ChangesSmoothly(healthPoint));
        }

        private void ChangeMaxHealth(float maxHealthPoint)
        {
            _healthBar.maxValue = maxHealthPoint;
        }

        private IEnumerator ChangesSmoothly(float healthPoint)
        {
            float speed = 50;

            while (_healthBar.value != healthPoint)
            {
                _healthBar.value = Mathf.MoveTowards(_healthBar.value, healthPoint, speed * Time.deltaTime);
                yield return null;
            }

            _changesSmoothly = null;
        }
    }
}