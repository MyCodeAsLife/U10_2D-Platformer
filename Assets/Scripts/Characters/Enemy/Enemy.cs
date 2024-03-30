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
            _controller.IsSkillUsed.Changed += BattleSystem.OnChangeSkillUsed;
            _controller.IsDirectionLeft.Changed += BattleSystem.OnChangeDirection;
        }

        private void OnDisable()
        {
            _controller.IsSkillUsed.Changed -= BattleSystem.OnChangeSkillUsed;
            _controller.IsDirectionLeft.Changed -= BattleSystem.OnChangeDirection;
        }
    }
}