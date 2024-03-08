using System;
using UnityEngine;

namespace Game
{
    public class Slash : MonoBehaviour
    {
        public event Action<IDamageble> OnHit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamageble>(out IDamageble enemy))
                OnHit?.Invoke(enemy);
        }
    }
}