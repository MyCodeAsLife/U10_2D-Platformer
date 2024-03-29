using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class AnimationController : MonoBehaviour
    {
        protected readonly int Running = Animator.StringToHash("isRunning");
        protected readonly int Grounded = Animator.StringToHash("isGrounded");

        protected Animator _animator;
        protected SpriteRenderer _spriteRenderer;

        protected virtual void OnSwitchGrounded(bool isGrounded)
        {
            _animator.SetBool(Grounded, isGrounded);
        }

        protected virtual void OnSwitchRunning(bool isRunning)
        {
            _animator.SetBool(Running, isRunning);
        }

        protected virtual void OnSwitchDirection(bool isLeft)
        {
            _spriteRenderer.flipX = isLeft;
        }
    }
}