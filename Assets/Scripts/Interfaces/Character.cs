using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, IDamageble
    {
        [SerializeField] private Slash _slashPrefab;

        private Health _health = new Health();

        private float _physicalResistance;
        private float _armor;
        private float _damage;
        private float _attackSpeed;

        protected bool IsAttack;

        private void Start()
        {
            _physicalResistance = 0.1f;
            _armor = 0.05f;
            _damage = 10f;
            _attackSpeed = 0.3f;
            IsAttack = false;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public float MaxHealtValue { get { return _health.MaxValue; } }
        public float HealthValue { get { return _health.Value; } }
        //public float Damage { get { return _damage; } }
        //public float AttackSpeed { get { return _attackSpeed; } }

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

        public void AttackChange(bool isAttack, bool flipX)         //
        {
            IsAttack = isAttack;

            if (IsAttack)
                StartCoroutine(Slash(flipX));
        }

        private IEnumerator Slash(bool flipX)
        {
            WaitForSeconds _delay = new WaitForSeconds(_attackSpeed);

            while (IsAttack)
            {
                Slash slash = Instantiate(_slashPrefab, transform.position, Quaternion.identity);
                slash.OnHit += Attack;

                if (flipX)
                    slash.transform.rotation = new Quaternion(slash.transform.rotation.x, 180, slash.transform.rotation.z, slash.transform.rotation.w);
                else
                    slash.transform.rotation = new Quaternion(slash.transform.rotation.x, 0, slash.transform.rotation.z, slash.transform.rotation.w);

                //slash.GetComponent<SpriteRenderer>().flipX = true;
                slash.gameObject.SetActive(true);

                Destroy(slash.gameObject, 0.1f);

                yield return _delay;
            }
        }

        private void Attack(IDamageble enemy)
        {
            enemy.TakeDamage(_damage);
        }
    }
}