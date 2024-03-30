using System;
using UnityEngine;

namespace Game
{
    public class Aidkit : PickUpItem
    {
        [SerializeField] private float _healtPoints;

        public override event Action PickedUp;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Character>(out Character character))
            {
                PickedUp?.Invoke();
                character.TakeHealing(_healtPoints);
            }
        }
    }
}