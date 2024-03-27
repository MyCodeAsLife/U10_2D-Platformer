using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(EnemyController))]
    public class Enemy : Character
    {
        private EnemyController _controller;

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<EnemyController>();
        }

        private void OnEnable()
        {
            _controller.PROPERTY_SKILL_USED.OnChanged += BattleSystem.ChangeSkillUsed;
            _controller.PROPERTY_DIRECTION.OnChanged += BattleSystem.ChangeDirection;
        }

        private void OnDisable()
        {
            _controller.PROPERTY_SKILL_USED.OnChanged -= BattleSystem.ChangeSkillUsed;
            _controller.PROPERTY_DIRECTION.OnChanged -= BattleSystem.ChangeDirection;
        }
    }
}