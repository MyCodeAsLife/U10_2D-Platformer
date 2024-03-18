using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(EnemyController))]
    public class AI : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerObstacle;
        [SerializeField] private LayerMask _layerEnemys;
        [SerializeField] private Transform _route;

        private EnemyController _enemyController;
        private List<Character> _enemies = new List<Character>();
        private Transform[] _waypoints;
        private Coroutine _currentBehavior;
        private Coroutine _stateSelection;

        private CharacterState _characterState;
        private float _attackRange;
        private int _nextPointIndex;
        private bool _hasEnemyInRadius;

        private void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
        }

        private void OnEnable()
        {
            _waypoints = new Transform[_route.childCount];

            for (int i = 0; i < _waypoints.Length; i++)
                _waypoints[i] = _route.GetChild(i);

            _enemyController.TargetPoint = _waypoints[_nextPointIndex].position;
            _characterState = CharacterState.Patrolling;
            _currentBehavior = StartCoroutine(Patrolling());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Start()
        {
            _hasEnemyInRadius = false;
            _attackRange = 1.5f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player enemy))
            {
                if (_enemies.Contains(enemy) == false)
                {
                    _enemies.Add(enemy);
                }

                if (_hasEnemyInRadius == false)
                {
                    _hasEnemyInRadius = true;

                    if (_stateSelection != null)
                        StopCoroutine(_stateSelection);

                    _stateSelection = StartCoroutine(StateSelection());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player enemy))
            {
                _enemies.Remove(enemy);

                if (_enemies.Count < 1)
                    _hasEnemyInRadius = false;
            }
        }

        private void ChoosePatrol()
        {
            if (_characterState == CharacterState.Attack)
            {
                if (_currentBehavior != null)
                    StopCoroutine(_currentBehavior);

                _enemyController.ChangeAttackState(false);
                _characterState = CharacterState.Patrolling;
                _currentBehavior = StartCoroutine(Patrolling());
            }
        }

        private void ChooseAggression()
        {
            if (_characterState == CharacterState.Patrolling)
            {
                if (_currentBehavior != null)
                    StopCoroutine(_currentBehavior);

                _characterState = CharacterState.Attack;
                _currentBehavior = StartCoroutine(Aggression());
            }
        }

        private void AttackTargetSelect()
        {
            if (_enemies.Count > 0)
            {
                _enemyController.TargetPoint = _enemies[0].transform.position;

                if (_enemies.Count > 1)
                {
                    float distanceEnemy1 = Vector2.Distance(transform.position, _enemyController.TargetPoint);

                    for (int i = 1; i < _enemies.Count; i++)
                    {
                        float distanceEnemy2 = Vector2.Distance(transform.position, _enemies[i].transform.position);

                        if (distanceEnemy2 < distanceEnemy1)
                            _enemyController.TargetPoint = _enemies[i].transform.position;
                    }
                }
            }
        }

        private IEnumerator Patrolling()
        {
            const float Second = 2f;
            bool isPatriling = true;
            var wait = new WaitForSeconds(Second);

            while (isPatriling)
            {
                if (_enemyController.DistanceToTargetX <= _enemyController.MinDistance)
                {
                    yield return wait;

                    _nextPointIndex = (_nextPointIndex + 1) % _waypoints.Length;
                    _enemyController.TargetPoint = _waypoints[_nextPointIndex].position;

                    _enemyController.MovementEnable();
                }

                yield return null;
            }
        }

        private IEnumerator Aggression()
        {
            float reactionDelay = 0.3f;
            var delay = new WaitForSeconds(reactionDelay);

            while (true)
            {
                AttackTargetSelect();
                _enemyController.MoveCalculate();
                float distanceToTarget = Vector2.Distance(transform.position, _enemyController.TargetPoint);

                if (_enemyController.DistanceToTargetX > _enemyController.MinDistance)
                    if (_enemyController.IsMove == false)
                        _enemyController.MovementEnable();

                _enemyController.ChangeAttackState(distanceToTarget < _attackRange);

                yield return delay;
            }
        }

        private IEnumerator StateSelection()
        {
            float seconds = 1f;
            var delay = new WaitForSeconds(seconds);
            float aggressionRadius = GetComponentInChildren<CircleCollider2D>().radius;
            bool isEnemyVisible = false;

            while (_hasEnemyInRadius)
            {
                for (int i = 0; i < _enemies.Count; i++)
                {
                    isEnemyVisible = false;
                    var hit = Physics2D.Raycast(transform.position, (_enemies[i].transform.position - transform.position).normalized, aggressionRadius, _layerObstacle);

                    if (hit)
                        isEnemyVisible = (_layerEnemys.value & (1 << hit.collider.gameObject.layer)) > 0;

                    if (isEnemyVisible && _characterState == CharacterState.Patrolling)
                    {
                        _enemyController.TargetPoint = hit.transform.position;
                        ChooseAggression();
                        break;
                    }
                }

                if (isEnemyVisible == false && _characterState == CharacterState.Attack)
                    ChoosePatrol();

                yield return delay;
            }

            ChoosePatrol();
            _stateSelection = null;
        }
    }
}