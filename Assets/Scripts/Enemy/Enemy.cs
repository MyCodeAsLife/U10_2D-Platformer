using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(EnemyController))]
    public class Enemy : Character
    {
        private EnemyController _controller;

        private void Awake()
        {
            _controller = GetComponent<EnemyController>();
        }

        private void OnEnable()
        {
            _controller.OnAttack += AttackChange;
        }

        private void OnDisable()
        {
            _controller.OnAttack -= AttackChange;
        }
    }
}