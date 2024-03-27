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
            _enemyController.PROPERTY_RUNNING.OnChanged += SwitchRunning;
            _enemyController.PROPERTY_DIRECTION.OnChanged += SwitchDirection;
        }

        private void OnDisable()
        {
            _enemyController.PROPERTY_RUNNING.OnChanged -= SwitchRunning;
            _enemyController.PROPERTY_DIRECTION.OnChanged -= SwitchDirection;
        }
    }
}