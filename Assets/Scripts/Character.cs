using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, IDamageble, IHealable
    {
        [SerializeField] private ISkill _slashPrefab;

        private ISkill _slash;
        //private Vampirism _vampirism;
        private Health _health;

        private Coroutine _strike;
        //private Coroutine _pumpOver;

        private float _physicalResistance;
        private float _armor;
        private float _damage;
        private float _attackSpeed;
        private float _visibilityTimeSlash;

        private bool _flipX;
        private bool _isAttackAttempt;
        private bool _canAttack;

        public float CurrentHealth { get { return _health.Value; } }
        public float PrecentCurrentHealth { get { return _health.PercentValue; } }
        public float MaxHealth { get { return _health.MaxValue; } }

        protected virtual void Awake()
        {
            _health = new Health(100);
        }

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

        public void SubscribeHeathChanged(Action<float> function)
        {
            _health.OnChange += function;
        }

        public void UnsubscribeHeathChanged(Action<float> function)
        {
            _health.OnChange -= function;
        }

        public void TakeDamage(float damage)
        {
            damage -= damage * _physicalResistance * _armor;
            damage -= damage * _armor;
            _health.Decrease(damage);
        }

        public void TakeHealing(float healthPoints)
        {
            _health.Increase(healthPoints);
        }

        public void ChangeAttackState(bool isAttackAttempt)
        {
            _isAttackAttempt = isAttackAttempt;

            if (_isAttackAttempt && _strike == null)
            {
                _slash.OnHit += Attack;
                _strike = StartCoroutine(Strike());
            }
            else if (_strike != null)
            {
                StopCoroutine(_strike);
                _slash.OnHit -= Attack;
                _canAttack = true;
                _strike = null;
            }
        }

        public void ChangeDirection(bool flipX)
        {
            _flipX = flipX;
        }

        private void Attack(IDamageble enemy)
        {
            enemy.TakeDamage(_damage);
        }

        private void DisableSlash()
        {
            _slash.gameObject.SetActive(false);
        }

        private IEnumerator Strike()
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

            _strike = null;
        }
    }
}