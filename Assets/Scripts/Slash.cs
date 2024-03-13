using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Slash : MonoBehaviour
    {
        [SerializeField] private ContactFilter2D _contactFilter;

        private Vector2 _affectedArea = new Vector2(2, 2);

        public event Action<IDamageble> OnHit;

        private void OnEnable()
        {
            List<Collider2D> hits = new List<Collider2D>();
            Physics2D.OverlapBox(transform.position, _affectedArea, 0, _contactFilter, hits);

            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<IDamageble>(out IDamageble obj))
                    OnHit?.Invoke(obj);
            }
        }
    }
}