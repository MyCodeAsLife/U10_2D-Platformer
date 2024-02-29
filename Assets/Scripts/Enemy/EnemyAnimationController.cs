using UnityEngine;

public class EnemyAnimationController : AnimationController
{
    private EnemyController _enemyController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyController = GetComponentInParent<EnemyController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _enemyController.OnRunning += SwitchRunning;
        _enemyController.OnDirection += SwitchDirection;
    }

    private void OnDisable()
    {
        _enemyController.OnRunning -= SwitchRunning;
        _enemyController.OnDirection -= SwitchDirection;
    }
}
