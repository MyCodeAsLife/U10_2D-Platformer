using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform _route;

        private List<Character> _enemies = new List<Character>();
        private Transform[] _waypoints;
        private Transform _targetPoint;
        //private Transform _targetAttack;

        //private bool _isRight;
        private bool _isEnemyFound;
        private bool _isMove;
        private int _nextPointIndex;

        private float _moveSpeed;
        private float _minDistance;
        private float _newPosX;
        private float _delayWithAttack;
        private float _distanceToTarget;

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
            //_isRight = false;
            _minDistance = 1.3f;
            _moveSpeed = 3f;
            _delayWithAttack = 0.5f;
            _waypoints = new Transform[_route.childCount];

            for (int i = 0; i < _waypoints.Length; i++)
                _waypoints[i] = _route.GetChild(i);

            _targetPoint = _waypoints[_nextPointIndex];
            StartCoroutine(Patrolling());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player enemy))
            {
                if (_enemies.Contains(enemy) == false)
                {
                    _enemies.Add(enemy);
                }

                if (_isEnemyFound == false)
                {
                    _isEnemyFound = true;
                    StopAllCoroutines();
                    StartCoroutine(ChangeAttackState());
                }
            }
        }

        private void Update()
        {
            onMoveUpdate?.Invoke();
        }

        private void Move()
        {
            //_newPosX = Mathf.MoveTowards(transform.position.x, _targetPoint.position.x, _moveSpeed * Time.deltaTime);
            //_isRight = (_newPosX - transform.position.x > 0);
            //OnDirection?.Invoke(_isRight);
            transform.position = new Vector2(_newPosX, transform.position.y);
        }

        private void Rotate()
        {
            bool _isRight = (_newPosX - transform.position.x > 0);
            OnDirection?.Invoke(_isRight);
        }

        private void MoveCalculate()
        {
            _newPosX = Mathf.MoveTowards(transform.position.x, _targetPoint.position.x, _moveSpeed * Time.deltaTime);
            //_distanceToTarget = Vector2.Distance(transform.position, _targetPoint.position);

            _distanceToTarget = transform.position.x - _targetPoint.position.x;

            if (_distanceToTarget < 0)
                _distanceToTarget *= -1;
        }

        private void Movement()
        {
            MoveCalculate();
            //_distanceToTarget = Vector2.Distance(transform.position, _targetPoint.position);
            //Debug.Log(_distanceToTarget);
            //Debug.Log("Movement");

            if (_distanceToTarget <= _minDistance)
            {
                _isMove = false;
                OnRunning?.Invoke(_isMove);
                onMoveUpdate -= Movement;
            }
            else
            {
                Rotate();
                Move();
                OnRunning?.Invoke(_isMove);
            }

        }

        private void AttackEnable(InputAction.CallbackContext obj)
        {
            OnAttack?.Invoke(true);
        }

        private void AttackDisable(InputAction.CallbackContext obj)
        {
            OnAttack?.Invoke(false);
        }

        private void AttackTargetSelect()
        {
            _targetPoint = _enemies[0].transform;

            if (_enemies.Count > 1)
            {
                float distanceEnemy1 = Vector2.Distance(transform.position, _targetPoint.position);

                for (int i = 1; i < _enemies.Count; i++)
                {
                    float distanceEnemy2 = Vector2.Distance(transform.position, _enemies[i].transform.position);

                    if (distanceEnemy2 < distanceEnemy1)
                        _targetPoint = _enemies[i].transform;
                }
            }
        }

        private IEnumerator Patrolling()                  // Перенести в Update через событие onMoveUpdate
        {
            const float Second = 3.5f;
            bool isPatriling = true;
            var wait = new WaitForSeconds(Second);

            while (isPatriling)
            {
                //MoveCalculate();
                //Rotate();
                //Move();
                //OnRunning?.Invoke(true);

                //_distanceToTarget = Vector2.Distance(transform.position, _targetPoint.position);

                if (_distanceToTarget <= _minDistance)
                {
                    yield return wait;
                    //OnRunning?.Invoke(false);
                    _nextPointIndex = (_nextPointIndex + 1) % _waypoints.Length;
                    _targetPoint = _waypoints[_nextPointIndex];

                    //if (_isMove == false)
                    //{
                    _isMove = true;
                    onMoveUpdate += Movement;
                    //}
                }

                yield return null;
            }
        }

        private IEnumerator ChangeAttackState()       // Добавить проверки на расстояние для потери врага из зоны видимости, атаку врага если он рядом(дистанция через вектор2 и ротате) и возврат к патрулированию
        {
            yield return new WaitForSeconds(_delayWithAttack);

            while (true)
            {
                AttackTargetSelect();

                if (_isMove == false)
                {
                    _isMove = true;
                    onMoveUpdate += Movement;
                }

                yield return new WaitForSeconds(_delayWithAttack);
            }
        }
    }
}