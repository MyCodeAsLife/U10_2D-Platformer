using UnityEngine;

namespace Game
{
    public class PlayerAnimationController : AnimationController
    {
        private PlayerMovementController _playerMovementController;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _playerMovementController = GetComponentInParent<PlayerMovementController>();
        }

        private void OnEnable()
        {
            _playerMovementController.IsGrounded.Changed += OnSwitchGrounded;
            _playerMovementController.IsRunning.Changed += OnSwitchRunning;
            _playerMovementController.IsDirectionLeft.Changed += OnSwitchDirection;
        }

        private void OnDisable()
        {
            _playerMovementController.IsGrounded.Changed -= OnSwitchGrounded;
            _playerMovementController.IsRunning.Changed -= OnSwitchRunning;
            _playerMovementController.IsDirectionLeft.Changed -= OnSwitchDirection;
        }
    }
}