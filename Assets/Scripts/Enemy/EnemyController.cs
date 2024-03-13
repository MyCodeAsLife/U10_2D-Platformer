using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private Transform _route;

        private List<Character> _enemies = new List<Character>();
        private Transform[] _waypoints;
        private Vector3 _targetPoint;
        private Coroutine _currentBehavior;

        private bool _isEnemyFound;
        private bool _isMove;
        private int _nextPointIndex;

        private float _moveSpeed;
        private float _minDistance;
        private float _newPosX;
        private float _delayWithAttack;
        private float _distanceToTargetX;

        public event Action<bool> OnRunning;
        public event Action<bool> OnDirection;
        public event Action<bool> OnAttack;

        private event Action onMoveUpdate;

        private void OnEnable()
        {
            _isMove = true;
            onMoveUpdate += Movement;
        }

        private void OnDisable()
        {
            onMoveUpdate -= Movement;
        }

        private void Start()
        {
            _isEnemyFound = false;
            _minDistance = 1.2f;
            _moveSpeed = 3f;
            _delayWithAttack = 0.5f;
            _waypoints = new Transform[_route.childCount];

            for (int i = 0; i < _waypoints.Length; i++)
                _waypoints[i] = _route.GetChild(i);

            _targetPoint = _waypoints[_nextPointIndex].position;
            ChoosePatrol();
        }

        private void OnTriggerEnter2D(Collider2D collision)                     // Добавить рейкаст для определения что цель в зоне видимости
        {
            if (collision.TryGetComponent<Player>(out Player enemy))
            {
                Vector2.
                Physics2D.Raycast(transform.position, /*((enemy.transform.position - transform.position).normalized)*/, );

                if (_enemies.Contains(enemy) == false)
                {
                    _enemies.Add(enemy);
                }

                if (_isEnemyFound == false)
                {
                    _isEnemyFound = true;
                    ChooseAttack();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player enemy))
            {
                Debug.Log("Exit");

                AttackTargetSelect();
                _enemies.Remove(enemy);
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
            if (_currentBehavior != null)
                StopCoroutine(_currentBehavior);

            _currentBehavior = StartCoroutine(Patrolling());
        }

        private void ChooseAttack()
        {
            if (_currentBehavior != null)
                StopCoroutine(_currentBehavior);

            _currentBehavior = StartCoroutine(Attack());
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
            const float Second = 3.5f;
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

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(_delayWithAttack);

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
                else
                {
                    float distanceToTarget = Vector2.Distance(transform.position, _targetPoint);

                    if (_enemies.Count > 0 && distanceToTarget < _minDistance)
                    {
                        AttackEnable();
                    }
                    else
                    {
                        _isEnemyFound = false;
                        ChoosePatrol();
                    }
                }

                yield return new WaitForSeconds(_delayWithAttack);
            }
        }
    }
}