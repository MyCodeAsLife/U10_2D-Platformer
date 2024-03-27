using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, IDamageble, IHealable
    {
        protected BattleSystem BattleSystem;

        [SerializeField] private List<ISkill> prefabSkillList;

        private Health _health;
        private float _physicalResistance;
        private float _armor;

        public float CurrentHealth { get { return _health.Value; } }
        public float PrecentCurrentHealth { get { return _health.PercentValue; } }
        public float MaxHealth { get { return _health.MaxValue; } }

        protected virtual void Awake()
        {
            _health = new Health(100);
            BattleSystem = this.gameObject.AddComponent<BattleSystem>();
            BattleSystem.SetPrefabSkillList(prefabSkillList);
        }

        private void Start()
        {
            _physicalResistance = 0.1f;
            _armor = 0.05f;
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
    }
}