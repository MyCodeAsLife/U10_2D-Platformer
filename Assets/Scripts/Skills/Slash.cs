using UnityEngine;

namespace Game
{
    public class Slash : Skill
    {
        public Slash() : base(SkillEnum.Slash)
        {
            Radius = 2f;
            Duration = 0.03f;
            RollbackTime = 0.05f;
            SkillEffects.Add(SkillEffectsEnum.Damage);
        }

        public override void Use()
        {
            Physics2D.OverlapBox(transform.position, new Vector2(Radius, Radius), 0, ContactFilter, Hits);
            base.Use();
        }
    }
}