using System;
using UnityEngine;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        private bool _isMove;

        private float _moveSpeed;
        private float _minDistance;
        private float _newPosX;
        private float _distanceToTargetX;

        public event Action<bool> OnRunning;
        public event Action<bool> OnDirection;
        public event Action<bool> OnAttack;
        public event Action onMoveUpdate;

        public Vector3 TargetPoint { get; set; }
        public bool IsMove { get { return _isMove; } }
        public float MinDistance { get { return _minDistance; } }
        public float DistanceToTargetX { get { return _distanceToTargetX; } }

        private void OnEnable()
        {
            SwitchMovement(true);
        }

        private void OnDisable()
        {
            onMoveUpdate -= Movement;
            StopAllCoroutines();
        }

        private void Start()
        {
            _minDistance = 1.2f;
            _moveSpeed = 3f;
        }

        private void Update()
        {
            onMoveUpdate?.Invoke();
        }

        public void ChangeAttackState(bool isAttack)
        {
            OnAttack?.Invoke(isAttack);
        }

        public void SwitchMovement(bool isMove)
        {
            _isMove = isMove;

            if (_isMove)
                onMoveUpdate += Movement;
            else
                onMoveUpdate -= Movement;
        }

        public void MoveCalculate()
        {
            _newPosX = Mathf.MoveTowards(transform.position.x, TargetPoint.x, _moveSpeed * Time.deltaTime);
            _distanceToTargetX = transform.position.x - TargetPoint.x;

            if (_distanceToTargetX < 0)
                _distanceToTargetX *= -1;
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

        private void Movement()
        {
            MoveCalculate();
            Rotate();

            if (_distanceToTargetX <= _minDistance)
                SwitchMovement(false);
            else
                Move();

            OnRunning?.Invoke(_isMove);
        }
    }
}