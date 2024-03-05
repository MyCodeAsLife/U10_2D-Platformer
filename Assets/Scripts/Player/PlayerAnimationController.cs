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
            _playerController.OnRunning += SwitchRunning;
            _playerController.OnGrounded += SwitchGrounded;
            _playerController.OnDirection += SwitchDirection;
        }

        private void OnDisable()
        {
            _playerController.OnRunning -= SwitchRunning;
            _playerController.OnGrounded -= SwitchGrounded;
            _playerController.OnDirection -= SwitchDirection;
        }
    }
}