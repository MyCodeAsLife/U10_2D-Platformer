using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(EnemyController))]
    public class Enemy : Character
    {
        private EnemyController _controller;

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponent<EnemyController>();
        }

        private void OnEnable()
        {
            _controller.OnAttack += ChangeAttackState;
            _controller.OnDirection += ChangeDirection;
        }

        private void OnDisable()
        {
            _controller.OnAttack -= ChangeAttackState;
            _controller.OnDirection -= ChangeDirection;
        }
    }
}