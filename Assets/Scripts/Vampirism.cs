using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

namespace Game
{
    public class Vampirism : ISkill
    {
        public override event Action<IInteractive, List<SkillEffectsEnum>> OnHit;

        public Vampirism() : base(SkillEnum.Vampirism)
        {
            Radius = 3f;
            Duration = 6f;
            RollbackTime = 6f;
            SkillEffects.Add(SkillEffectsEnum.Vampirism);
        }

        public override void Use()
        {
            Physics2D.OverlapCircle(transform.position, Radius, ContactFilter, hits);

            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<IDamageble>(out IDamageble obj) && IsReady)
                {
                    IsReady = false;
                    StartCoroutine(UsingSkill(hit));
                    break;
                }
            }
        }

        private IEnumerator UsingSkill(Collider2D obj)
        {
            float time = 0;
            float tickTime = 0.3f;
            var tick = new WaitForSeconds(tickTime);
            obj.TryGetComponent<IDamageble>(out IDamageble enemy);

            while (time < Duration)
            {
                if (Vector2.Distance(obj.transform.position, Owner.position) > Radius)
                    break;

                OnHit?.Invoke(enemy, SkillEffects);
                time += tickTime;
                yield return tick;
            }
        }
    }
}