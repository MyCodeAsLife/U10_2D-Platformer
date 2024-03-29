using UnityEngine;

namespace Game
{
    public class PlayerAnimationController : AnimationController
    {
        private PlayerController _playerController;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerController = GetComponentInParent<PlayerController>();
        }

        private void OnEnable()
        {
            _playerController.IsRunning.Changed += OnSwitchRunning;
            _playerController.IsGrounded.Changed += OnSwitchGrounded;
            _playerController.IsDirectionLeft.Changed += OnSwitchDirection;
        }

        private void OnDisable()
        {
            _playerController.IsRunning.Changed -= OnSwitchRunning;
            _playerController.IsGrounded.Changed -= OnSwitchGrounded;
            _playerController.IsDirectionLeft.Changed -= OnSwitchDirection;
        }
    }
}