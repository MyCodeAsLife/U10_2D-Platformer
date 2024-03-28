using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private Character _character;
    private List<ISkill> _skillList = new();
    private ISkill _currentSkill;
    private Coroutine _usingSkill;

    private bool _flipX;
    private bool _isUseSkillAttempt;
    private bool _canUseSkill;

    private void Start()
    {
        _character = GetComponent<Character>();
        _flipX = false;
        _isUseSkillAttempt = false;
        _canUseSkill = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetPrefabSkillList(List<ISkill> prefabSkillList)
    {
        for (int i = 0; i < prefabSkillList.Count; i++)
        {
            _skillList.Add(Instantiate(prefabSkillList[i]));
            _skillList[i].SetOwner(transform);
        }
    }

    public void ChangeSkillUsed(bool isAttackAttempt, SkillEnum skillName)
    {
        if (_currentSkill != null)
        {
            _currentSkill.OnHit -= ImpactToTarget;
            _currentSkill = null;
        }

        _isUseSkillAttempt = isAttackAttempt;

        for (int i = 0; i < _skillList.Count; i++)
        {
            if (_skillList[i].NAME == skillName)
            {
                _currentSkill = _skillList[i];
                _currentSkill.OnHit += ImpactToTarget;
            }
        }

        if (_currentSkill == null)
            throw new NullReferenceException("Skill not found: " + skillName);

        ChangeAttackState();
    }

    public void ChangeAttackState()
    {
        if (_isUseSkillAttempt && _usingSkill == null)
            _usingSkill = StartCoroutine(UsingSkill());
    }

    public void ChangeDirection(bool flipX)
    {
        _flipX = flipX;
    }

    private void ImpactToTarget(IInteractive target, List<SkillEffectsEnum> skillEffects)
    {
        float damageDone = 0;

        if (target is IDamageble)
            damageDone = (target as IDamageble).TakeDamage(_character.Damage);

        if (skillEffects.Contains(SkillEffectsEnum.Vampirism))
            _character.TakeHealing(damageDone);
    }

    private IEnumerator UsingSkill()
    {
        WaitForSeconds _delay = new WaitForSeconds(_character.CastSpeed);

        while (_isUseSkillAttempt && _canUseSkill)
        {
            if (_currentSkill.IsReady)
            {
                _canUseSkill = false;
                _currentSkill.transform.position = transform.position;

                if (_flipX)
                    _currentSkill.transform.localScale = new Vector3(-1, _currentSkill.transform.localScale.y, _currentSkill.transform.localScale.z);
                else
                    _currentSkill.transform.localScale = new Vector3(1, _currentSkill.transform.localScale.y, _currentSkill.transform.localScale.z);

                _currentSkill.Use();
            }

            yield return _delay;
            _canUseSkill = true;
        }

        _usingSkill = null;
        _canUseSkill = true;
    }
}
