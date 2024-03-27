using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Slash : ISkill
    {
        public override event Action<IDamageble> OnHit;

        //public override Skill Name { get; private set; }

        public Slash() : base(Skill.Slash)
        {
            //Name = Skill.Slash;
            AffectedArea = 2f;
        }

        private void OnEnable()
        {
            List<Collider2D> hits = new List<Collider2D>();
            Physics2D.OverlapBox(transform.position, new Vector2(AffectedArea, AffectedArea), 0, ContactFilter, hits);

            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<IDamageble>(out IDamageble obj))
                    OnHit?.Invoke(obj);
            }
        }
    }
}