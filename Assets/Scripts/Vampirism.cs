using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Vampirism : ISkill
    {
        public override event Action<IDamageble> OnHit;

        public Vampirism() : base(Skill.Vampirism)
        {
            AffectedArea = 3f;
        }

        private void OnEnable()
        {
            List<Collider2D> hits = new List<Collider2D>();
            //Physics2D.OverlapBox(transform.position, _radius, 0, _contactFilter, hits);
            Physics2D.OverlapCircle(transform.position, AffectedArea, ContactFilter, hits);

            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<IDamageble>(out IDamageble obj))
                    OnHit?.Invoke(obj);
            }
        }
    }
}