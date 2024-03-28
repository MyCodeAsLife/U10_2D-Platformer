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

        public float Damage { get; private set; }
        public float CastSpeed { get; private set; }
        public float MaxHealth { get { return _health.MaxValue; } }
        public float CurrentHealth { get { return _health.Value; } }
        public float PrecentCurrentHealth { get { return _health.PercentValue; } }

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
            Damage = 15f;
            CastSpeed = 0.6f;
        }

        public void SubscribeHeathChanged(Action<float> function)
        {
            _health.OnChange += function;
        }

        public void UnsubscribeHeathChanged(Action<float> function)
        {
            _health.OnChange -= function;
        }

        public float TakeDamage(float damage)
        {
            damage -= damage * _physicalResistance * _armor;
            damage -= damage * _armor;
            _health.Decrease(damage);
            return damage;
        }

        public void TakeHealing(float healthPoints)
        {
            _health.Increase(healthPoints);
        }
    }
}