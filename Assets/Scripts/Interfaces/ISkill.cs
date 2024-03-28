using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class ISkill : MonoBehaviour
    {
        public readonly SkillEnum NAME;

        [SerializeField] protected ContactFilter2D ContactFilter;

        protected List<Collider2D> hits = new List<Collider2D>();
        protected Transform Owner;
        protected float Radius;

        private float _currentTime;

        public virtual event Action<IInteractive, List<SkillEffectsEnum>> OnHit;

        public bool IsReady { get; protected set; }
        public float Duration { get; protected set; }
        public List<SkillEffectsEnum> SkillEffects { get; protected set; }
        public float RollbackTime { get; protected set; }

        public ISkill(SkillEnum skillName)
        {
            NAME = skillName;
            IsReady = true;
            SkillEffects = new List<SkillEffectsEnum>();
        }

        private void FixedUpdate()
        {
            if (IsReady == false)
                Rollingback();
        }

        public virtual void Use()
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<IInteractive>(out IInteractive obj))
                {
                    OnHit?.Invoke(obj, SkillEffects);
                    IsReady = false;
                }
            }

            StartCoroutine(Renderer());
        }

        public void SetOwner(Transform owner)
        {
            Owner = owner;
        }

        private void Rollingback()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= RollbackTime)
            {
                _currentTime = 0f;
                IsReady = true;
            }
        }

        private IEnumerator Renderer()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            var delay = new WaitForSeconds(Duration);
            float time = 0;

            while (time < Duration)
            {
                time += Time.deltaTime;
                yield return delay;
            }

            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}