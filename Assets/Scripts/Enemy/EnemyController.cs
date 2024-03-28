using System;
using UnityEngine;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        public readonly SingleReactiveProperty<bool> PROPERTY_RUNNING = new();
        public readonly SingleReactiveProperty<bool> PROPERTY_DIRECTION = new();
        public readonly DoubleReactiveProperty<bool, SkillEnum> PROPERTY_SKILL_USED = new();

        private float _moveSpeed;
        private float _minDistance;
        private float _newPosX;
        private float _distanceToTargetX;

        public event Action onMoveUpdate;

        public Vector3 TargetPoint { get; set; }
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
            PROPERTY_SKILL_USED.SetValues(isAttack, SkillEnum.Slash);
        }

        public void SwitchMovement(bool isMove)
        {
            PROPERTY_RUNNING.Value = isMove;

            if (PROPERTY_RUNNING.Value)
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
            PROPERTY_DIRECTION.Value = _newPosX - transform.position.x > 0;
        }

        private void Movement()
        {
            MoveCalculate();
            Rotate();

            if (_distanceToTargetX <= _minDistance)
                SwitchMovement(false);
            else
                Move();
        }
    }
}