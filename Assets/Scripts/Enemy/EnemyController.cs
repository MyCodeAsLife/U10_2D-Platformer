using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerObstacle;
        [SerializeField] private LayerMask _layerEnemys;
        [SerializeField] private Transform _route;

        private List<Character> _enemies = new List<Character>();
        private Transform[] _waypoints;
        private Vector3 _targetPoint;
        private Coroutine _currentBehavior;
        private Coroutine _stateSelection;

        private CharacterState _characterState;
        private int _nextPointIndex;

        private bool _isEnemyInRadius;
        private bool _isEnemyVisible;
        private bool _isMove;

        private float _moveSpeed;
        private float _minDistance;
        private float _newPosX;
        private float _distanceToTargetX;

        public event Action<bool> OnRunning;
        public event Action<bool> OnDirection;
        public event Action<bool> OnAttack;

        private event Action onMoveUpdate;

        private void OnEnable()
        {
            _isMove = true;
            onMoveUpdate += Movement;
            _waypoints = new Transform[_route.childCount];

            for (int i = 0; i < _waypoints.Length; i++)
                _waypoints[i] = _route.GetChild(i);

            _targetPoint = _waypoints[_nextPointIndex].position;
            _characterState = CharacterState.Patrolling;
            _currentBehavior = StartCoroutine(Patrolling());
        }

        private void OnDisable()
        {
            onMoveUpdate -= Movement;
            StopAllCoroutines();
        }

        private void Start()
        {
            _isEnemyInRadius = false;
            _isEnemyVisible = false;
            _minDistance = 1.2f;
            _moveSpeed = 3f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player enemy))
            {
                if (_enemies.Contains(enemy) == false)
                {
                    _enemies.Add(enemy);
                }

                if (_isEnemyInRadius == false)
                {
                    _isEnemyInRadius = true;

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
                    _isEnemyInRadius = false;
            }
        }

        private void Update()
        {
            onMoveUpdate?.Invoke();
        }

        private void Move()
        {
            transform.position = new Vector2(_newPosX, transform.position.y);
        }

        private void Rotate()
        {
            bool _isRight = (_newPosX - transform.position.x > 0);
            OnDirection?.Invoke(_isRight);
        }

        private void MoveCalculate()
        {
            _newPosX = Mathf.MoveTowards(transform.position.x, _targetPoint.x, _moveSpeed * Time.deltaTime);
            _distanceToTargetX = transform.position.x - _targetPoint.x;

            if (_distanceToTargetX < 0)
                _distanceToTargetX *= -1;
        }

        private void Movement()
        {
            MoveCalculate();
            Rotate();

            if (_distanceToTargetX <= _minDistance)
            {
                _isMove = false;
                OnRunning?.Invoke(_isMove);
                onMoveUpdate -= Movement;
            }
            else
            {
                Move();
                OnRunning?.Invoke(_isMove);
            }
        }

        private void AttackEnable()
        {
            OnAttack?.Invoke(true);
        }

        private void AttackDisable()
        {
            OnAttack?.Invoke(false);
        }

        private void ChoosePatrol()
        {
            if (_characterState == CharacterState.Attack)
            {
                if (_currentBehavior != null)
                    StopCoroutine(_currentBehavior);

                AttackDisable();
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
                _targetPoint = _enemies[0].transform.position;

                if (_enemies.Count > 1)
                {
                    float distanceEnemy1 = Vector2.Distance(transform.position, _targetPoint);

                    for (int i = 1; i < _enemies.Count; i++)
                    {
                        float distanceEnemy2 = Vector2.Distance(transform.position, _enemies[i].transform.position);

                        if (distanceEnemy2 < distanceEnemy1)
                            _targetPoint = _enemies[i].transform.position;
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
                if (_distanceToTargetX <= _minDistance)
                {
                    yield return wait;

                    _nextPointIndex = (_nextPointIndex + 1) % _waypoints.Length;
                    _targetPoint = _waypoints[_nextPointIndex].position;
                    _isMove = true;
                    onMoveUpdate += Movement;
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
                MoveCalculate();

                if (_distanceToTargetX > _minDistance)
                {
                    AttackDisable();

                    if (_isMove == false)
                    {
                        _isMove = true;
                        onMoveUpdate += Movement;
                    }
                }

                float distanceToTarget = Vector2.Distance(transform.position, _targetPoint);

                if (distanceToTarget < _minDistance)
                    AttackEnable();
                else
                    AttackDisable();

                yield return delay;
            }
        }

        private IEnumerator StateSelection()
        {
            float seconds = 1f;
            var delay = new WaitForSeconds(seconds);
            float aggressionRadius = GetComponentInChildren<CircleCollider2D>().radius;

            while (_isEnemyInRadius)
            {
                for (int i = 0; i < _enemies.Count; i++)
                {
                    _isEnemyVisible = false;
                    var hit = Physics2D.Raycast(transform.position, (_enemies[i].transform.position - transform.position).normalized, aggressionRadius, _layerObstacle);

                    if (hit)
                        _isEnemyVisible = (_layerEnemys.value & (1 << hit.collider.gameObject.layer)) > 0;

                    if (_isEnemyVisible && _characterState == CharacterState.Patrolling)
                    {
                        _targetPoint = hit.transform.position;
                        ChooseAggression();
                        break;
                    }
                }

                if (_isEnemyVisible == false && _characterState == CharacterState.Attack)
                    ChoosePatrol();

                yield return delay;
            }

            ChoosePatrol();
            _stateSelection = null;
        }

        private enum CharacterState
        {
            Patrolling,
            Attack,
        }
    }
}