using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private List<ISkill> _skillList = new();
    private ISkill _currentSkill;
    private Coroutine _strike;

    private float _damage;
    private float _attackSpeed;
    private float _visibilityTimeSlash;

    private bool _flipX;
    private bool _isAttackAttempt;
    private bool _canAttack;

    private void Start()
    {
        _damage = 10f;
        _attackSpeed = 0.4f;
        _visibilityTimeSlash = 0.1f;
        _flipX = false;
        _isAttackAttempt = false;
        _canAttack = true;
    }

    public void SetPrefabSkillList(List<ISkill> prefabSkillList)
    {
        for (int i = 0; i < prefabSkillList.Count; i++)
            _skillList.Add(Instantiate(prefabSkillList[i]));
    }

    public void ChangeSkillUsed(bool isAttackAttempt, Skill skillName)
    {
        _currentSkill = null;
        _isAttackAttempt = isAttackAttempt;

        for (int i = 0; i < _skillList.Count; i++)
        {
            if (_skillList[i].NAME == skillName)
                _currentSkill = _skillList[i];
        }

        if (_currentSkill == null)
            throw new NullReferenceException("Skill not found: " + skillName);

        ChangeAttackState();
    }

    public void ChangeAttackState()
    {
        if (_isAttackAttempt && _strike == null)
        {
            _currentSkill.OnHit += Attack;
            _strike = StartCoroutine(Strike());
        }
        else if (_strike != null)
        {
            StopCoroutine(_strike);
            _currentSkill.OnHit -= Attack;
            _canAttack = true;
            _strike = null;
        }
    }

    public void ChangeDirection(bool flipX)
    {
        _flipX = flipX;
    }

    private void Attack(IDamageble enemy)               // Переделать под прием эффектов скила. и выбор действия в зависимости от эффектов.
    {
        enemy.TakeDamage(_damage);                      // Сделать возврат "прошедшего по здоровью" урона, для вампиризма и/или для чегонить еще
    }

    private void DisableSlash()
    {
        _currentSkill.gameObject.SetActive(false);
    }

    private IEnumerator Strike()
    {
        WaitForSeconds _delay = new WaitForSeconds(_attackSpeed);

        while (_isAttackAttempt && _canAttack)
        {
            _canAttack = false;
            _currentSkill.transform.position = transform.position;

            if (_flipX)
                _currentSkill.transform.localScale = new Vector3(-1, _currentSkill.transform.localScale.y, _currentSkill.transform.localScale.z);
            else
                _currentSkill.transform.localScale = new Vector3(1, _currentSkill.transform.localScale.y, _currentSkill.transform.localScale.z);

            _currentSkill.gameObject.SetActive(true);
            Invoke(nameof(DisableSlash), _visibilityTimeSlash);
            yield return _delay;
            _canAttack = true;
        }

        _strike = null;
    }
}
