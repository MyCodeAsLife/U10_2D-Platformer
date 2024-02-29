using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    private PlayerController _playerController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

