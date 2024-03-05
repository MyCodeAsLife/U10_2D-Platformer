using System;
using UnityEngine;

namespace Game
{
    public class Aidkit : MonoBehaviour
    {
        [SerializeField] private float _healtPoints = 30;

        public event Action OnPickup;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Health>(out Health character))
            {
                OnPickup?.Invoke();
                character.Healing(_healtPoints);
            }
        }
    }
}