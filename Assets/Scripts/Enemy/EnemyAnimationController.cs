using UnityEngine;

namespace Game
{
    public class EnemyAnimationController : AnimationController
    {
        private EnemyController _enemyController;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _enemyController = GetComponentInParent<EnemyController>();
        }

        private void OnEnable()
        {
            _enemyController.IsRunning.Changed += OnSwitchRunning;
            _enemyController.IsDirectionLeft.Changed += OnSwitchDirection;
        }

        private void OnDisable()
        {
            _enemyController.IsRunning.Changed -= OnSwitchRunning;
            _enemyController.IsDirectionLeft.Changed -= OnSwitchDirection;
        }
    }
}