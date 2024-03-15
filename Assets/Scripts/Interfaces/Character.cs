using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, IDamageble
    {
        [SerializeField] private Slash _slashPrefab;

        private Slash _slash;
        private Health _health = new Health();
        private Coroutine _attack;

        private float _physicalResistance;
        private float _armor;
        private float _damage;
        private float _attackSpeed;
        private float _visibilityTimeSlash;

        private bool _flipX;
        private bool _isAttackAttempt;
        private bool _canAttack;

        private void Start()
        {
            _physicalResistance = 0.1f;
            _armor = 0.05f;
            _damage = 10f;
            _attackSpeed = 0.4f;
            _visibilityTimeSlash = 0.1f;
            _flipX = false;
            _isAttackAttempt = false;
            _canAttack = true;
            _slash = Instantiate(_slashPrefab);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public float MaxHealtValue { get { return _health.MaxValue; } }
        public float HealthValue { get { return _health.Value; } }

        public void HealChangeSubscribe(Action<float> function)
        {
            _health.HealthChanged += function;
        }

        public void HealChangeUnsubscribe(Action<float> function)
        {
            _health.HealthChanged -= function;
        }

        public void TakeDamage(float damage)
        {
            damage -= damage * _physicalResistance * _armor;
            damage -= damage * _armor;
            _health.Decrease(damage);
        }

        public void Healing(float healthPoints)
        {
            _health.Increase(healthPoints);
        }

        public void ChangeAttackState(bool isAttackAttempt)
        {
            _isAttackAttempt = isAttackAttempt;

            if (_isAttackAttempt && _attack == null)
            {
                _slash.OnHit += SetDamage;
                _attack = StartCoroutine(Attack());
            }
            else if (_attack != null)
            {
                StopCoroutine(_attack);
                _slash.OnHit -= SetDamage;
                _canAttack = true;
                _attack = null;
            }
        }

        public void ChangeDirection(bool flipX)
        {
            _flipX = flipX;
        }

        private IEnumerator Attack()
        {
            WaitForSeconds _delay = new WaitForSeconds(_attackSpeed);

            while (_isAttackAttempt && _canAttack)
            {
                _canAttack = false;
                _slash.transform.position = transform.position;

                if (_flipX)
                    _slash.transform.localScale = new Vector3(-1, _slash.transform.localScale.y, _slash.transform.localScale.z);
                else
                    _slash.transform.localScale = new Vector3(1, _slash.transform.localScale.y, _slash.transform.localScale.z);

                _slash.gameObject.SetActive(true);
                Invoke(nameof(DisableSlash), _visibilityTimeSlash);
                yield return _delay;
                _canAttack = true;
            }

            _attack = null;
        }

        private void SetDamage(IDamageble enemy)
        {
            enemy.TakeDamage(_damage);
        }

        private void DisableSlash()
        {
            _slash.gameObject.SetActive(false);
        }
    }
}