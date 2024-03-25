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
        }

        private void Start()
        {
            _character.SubscribeHeathChanged(ChangeHealth);
            ChangeHealth(_character.PrecentCurrentHealth);
        }

        private void ChangeHealth(float percentHealth)
        {
            if (_changesSmoothly != null)
                StopCoroutine(_changesSmoothly);

            _changesSmoothly = StartCoroutine(ChangesSmoothly(percentHealth));
        }

        private IEnumerator ChangesSmoothly(float healthPoint)
        {
            float speed = 0.7f;

            while (_healthBar.value != healthPoint)
            {
                _healthBar.value = Mathf.MoveTowards(_healthBar.value, healthPoint, speed * Time.deltaTime);
                yield return null;
            }

            _changesSmoothly = null;
        }
    }
}