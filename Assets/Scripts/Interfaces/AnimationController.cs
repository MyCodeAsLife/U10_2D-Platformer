using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class AnimationController : MonoBehaviour
{
    protected readonly int Running = Animator.StringToHash("isRunning");
    protected readonly int Grounded = Animator.StringToHash("isGrounded");

    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected virtual void SwitchGrounded(bool isGrounded)
    {
        _animator.SetBool(Grounded, isGrounded);
    }

    protected virtual void SwitchRunning(bool isRunning)
    {
        _animator.SetBool(Running, isRunning);
    }

    protected virtual void SwitchDirection(bool isLeft)
    {
        _spriteRenderer.flipX = isLeft;
    }
}
