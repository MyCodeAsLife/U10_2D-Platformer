using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerMovementController))]
    public class Player : Character
    {
        private PlayerInputController _controller;
        private PlayerMovementController _movementController;

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<PlayerInputController>();
            _movementController = GetComponent<PlayerMovementController>();
        }

        private void OnEnable()
        {
            _controller.IsSkillUsed.Changed += BattleSystem.OnChangeSkillUsed;
            _movementController.IsDirectionLeft.Changed += BattleSystem.OnChangeDirection;
        }

        private void OnDisable()
        {
            _controller.IsSkillUsed.Changed -= BattleSystem.OnChangeSkillUsed;
            _movementController.IsDirectionLeft.Changed -= BattleSystem.OnChangeDirection;
        }
    }
}
