using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class Gem : MonoBehaviour
    {
        private readonly int ShineTrigger = Animator.StringToHash("isShine");

        private Animator _animator;

        public event Action PickedUp;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            StartCoroutine(Shine());
        }

        private void OnDisable()
        {
            StopCoroutine(Shine());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerController>(out PlayerController enemy))
                PickedUp?.Invoke();
        }

        private IEnumerator Shine()
        {
            const float Second = 3f;
            var wait = new WaitForSeconds(Second);

            while (true)
            {
                _animator.SetTrigger(ShineTrigger);
                yield return wait;
            }
        }
    }
}