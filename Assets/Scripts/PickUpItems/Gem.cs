using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class Gem : PickUpItem
    {
        private readonly int ShineTrigger = Animator.StringToHash("isShine");

        private Animator _animator;

        public override event Action PickedUp;

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
            if (collision.TryGetComponent<PlayerInputController>(out PlayerInputController enemy))
                PickedUp?.Invoke();
        }

        private IEnumerator Shine()
        {
            const float Second = 3f;
            var wait = new WaitForSeconds(Second);
            bool isShine = true;

            while (isShine)
            {
                _animator.SetTrigger(ShineTrigger);
                yield return wait;
            }
        }
    }
}