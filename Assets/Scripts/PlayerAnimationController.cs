using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimationController : MonoBehaviour
{
    private const string Running = "isRunning";
    private const string Grounded = "isGrounded";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
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

    private void SwitchGrounded(bool isGrounded)
    {
        _animator.SetBool(Grounded, isGrounded);
    }

    private void SwitchRunning(bool isRunning)
    {
        _animator.SetBool(Running, isRunning);
    }

    private void SwitchDirection(bool isLeft)
    {
        _spriteRenderer.flipX = isLeft;
    }
}

