using System;
using UnityEngine;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        public readonly SingleReactiveProperty<bool> IsRunning = new();
        public readonly SingleReactiveProperty<bool> IsDirectionLeft = new();
        public readonly DoubleReactiveProperty<bool, SkillEnum> IsSkillUsed = new();

        private float _moveSpeed;
        private float _minDistance;
        private float _newPosX;
        private float _distanceToTargetX;

        public event Action MovementUpdate;

        public Vector3 TargetPoint { get; set; }
        public float MinDistance { get { return _minDistance; } }
        public float DistanceToTargetX { get { return _distanceToTargetX; } }

        private void OnEnable()
        {
            SwitchMovement(true);
        }

        private void OnDisable()
        {
            MovementUpdate -= OnMovementUpdate;
        }

        private void Start()
        {
            _minDistance = 1.2f;
            _moveSpeed = 3f;
        }

        private void Update()
        {
            MovementUpdate?.Invoke();
        }

        public void ChangeAttackState(bool isAttack)
        {
            IsSkillUsed.SetValues(isAttack, SkillEnum.Slash);
        }

        public void SwitchMovement(bool isMove)
        {
            IsRunning.Value = isMove;

            if (IsRunning.Value)
                MovementUpdate += OnMovementUpdate;
            else
                MovementUpdate -= OnMovementUpdate;
        }

        public void CalculateMovement()
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
            IsDirectionLeft.Value = _newPosX - transform.position.x > 0;
        }

        private void OnMovementUpdate()
        {
            CalculateMovement();
            Rotate();

            if (_distanceToTargetX <= _minDistance)
                SwitchMovement(false);
            else
                Move();
        }
    }
}