using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

namespace Game
{
    public class Vampirism : Skill
    {
        private Coroutine _usingSkill;

        public override event Action<IInteractive, List<SkillEffectsEnum>> Hited;

        public Vampirism() : base(SkillEnum.Vampirism)
        {
            Radius = 3f;
            Duration = 6f;
            RollbackTime = 6f;
            _usingSkill = null;
            SkillEffects.Add(SkillEffectsEnum.Vampirism);
        }

        public override void Use()
        {
            Physics2D.OverlapCircle(transform.position, Radius, ContactFilter, Hits);

            foreach (Collider2D hit in Hits)
            {
                if (hit.TryGetComponent<IDamageble>(out IDamageble obj) && IsReady)
                {
                    IsReady = false;

                    if (_usingSkill == null)
                        _usingSkill = StartCoroutine(UsingSkill(hit));

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

                Hited?.Invoke(enemy, SkillEffects);
                time += tickTime;
                yield return tick;
            }

            _usingSkill = null;
        }
    }
}