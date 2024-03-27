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
            _playerController.PROPERTY_RUNNING.OnChanged += SwitchRunning;
            _playerController.PROPERTY_GROUNDED.OnChanged += SwitchGrounded;
            _playerController.PROPERTY_DIRECTION.OnChanged += SwitchDirection;
        }

        private void OnDisable()
        {
            _playerController.PROPERTY_RUNNING.OnChanged -= SwitchRunning;
            _playerController.PROPERTY_GROUNDED.OnChanged -= SwitchGrounded;
            _playerController.PROPERTY_DIRECTION.OnChanged -= SwitchDirection;
        }
    }
}