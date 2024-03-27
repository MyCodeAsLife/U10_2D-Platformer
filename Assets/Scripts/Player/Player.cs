using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : Character
    {
        private PlayerController _controller;

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<PlayerController>();
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
