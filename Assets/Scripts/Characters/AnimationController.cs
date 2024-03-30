using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class AnimationController : MonoBehaviour
    {
        protected readonly int Running = Animator.StringToHash("isRunning");
        protected readonly int Grounded = Animator.StringToHash("isGrounded");

        protected Animator Animator;
        protected SpriteRenderer SpriteRenderer;

        protected virtual void OnSwitchGrounded(bool isGrounded)
        {
            Animator.SetBool(Grounded, isGrounded);
        }

        protected virtual void OnSwitchRunning(bool isRunning)
        {
            Animator.SetBool(Running, isRunning);
        }

        protected virtual void OnSwitchDirection(bool isLeft)
        {
            SpriteRenderer.flipX = isLeft;
        }
    }
}