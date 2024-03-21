using System;
using UnityEngine;

namespace Game
{
    public class Aidkit : MonoBehaviour
    {
        [SerializeField] private float _healtPoints;

        public event Action OnPickup;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Character>(out Character character))
            {
                OnPickup?.Invoke();
                character.TakeHealing(_healtPoints);
            }
        }
    }
}