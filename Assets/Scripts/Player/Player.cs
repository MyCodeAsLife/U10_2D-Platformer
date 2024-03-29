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
