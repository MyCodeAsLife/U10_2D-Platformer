using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class ISkill : MonoBehaviour
    {
        public readonly Skill NAME;
        public List<SkillEffect> _skillEffects = new();                     // Создать заполнение эфектами скилла при его создании

        [SerializeField] protected ContactFilter2D ContactFilter;

        protected float AffectedArea;

        public abstract event Action<IDamageble> OnHit;                     // Переделать чтобы он возвращал также список эфектов скила

        //public virtual Skill Name { get; set; }

        public ISkill(Skill skillName)
        {
            NAME = skillName;
        }
    }
}