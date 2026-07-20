using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillController : MonoBehaviour
{
    private GameObject currentSkillEffect;
    public ActiveSkill[] EquipedSkills { get; private set; }
    private PlayerBase playerBase;
    private Animator animator;
    private KeyCode[] skillKeys = { KeyCode.A, KeyCode.S, KeyCode.D };
    public int skillIndex = 0;

    private void Start()
    {
        playerBase = GameManager.Instance.PlayerBase;
        EquipedSkills = new ActiveSkill[3];
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        for(int i = 0; i < skillKeys.Length; i++)
        {
            if (Input.GetKeyDown(skillKeys[i]))
            {
                if (EquipedSkills[i] == null || !EquipedSkills[i].IsReady) { return; }
                TryUseSkill(EquipedSkills[i], i);
            }
        }
    }

    private void TryUseSkill(ActiveSkill activeSkill, int index)
    {
        currentSkillEffect = activeSkill.ActiveData.skillEffectPrefab;
        switch (activeSkill.ActiveData.activeType)
        {
            case ActiveSkillData.ActiveType.projectile:
                if (playerBase.currentWeapon.weaponType == WeaponData.WeaponType.Sword)
                {
                    AudioManager.instance.PlaySFX(SFX.Skill_Slash);
                    animator.SetTrigger("Projectile");
                }
                break;
            case ActiveSkillData.ActiveType.Roll:
                animator.SetTrigger("Rolling");
                break;
            case ActiveSkillData.ActiveType.Buff:
                PlayBuffParticle();
                activeSkill.ApplyBuff(playerBase);
                UIManager.Instance.ResisterBuff(activeSkill, () => activeSkill.ReleaseBuff(playerBase));
                AudioManager.instance.PlaySFX(SFX.Buff);
                break;
            default:
                break;
        }
        StartCoroutine(SkillCoolTime(activeSkill, index));
    }
    public void EquipSkill(ActiveSkill activeSkill)
    {
        for (int i = 0; i < EquipedSkills.Length; i++)
        {
            if (EquipedSkills[i] == null)
            {
                EquipedSkills[i] = activeSkill;
                UIManager.Instance.RegisterSkill(i, activeSkill);
                return;
            }
        }
    }
    public void EquipSkillFromDrag(int index, Skill skill)
    {
        if (skill is ActiveSkill activeSkill)
        {
            for(int i = 0; i < EquipedSkills.Length; i++)
            {
                if (EquipedSkills[i] == activeSkill)
                {
                    EquipedSkills[i] = null;
                    UIManager.Instance.RegisterSkill(i, null);
                }
            }
            EquipedSkills[index] = activeSkill;
            UIManager.Instance.RegisterSkill(index, activeSkill);
        }
    }
    public void PlaySkillParticle()
    {
        GameObject prefab = Instantiate(currentSkillEffect);
        if (prefab)
        {
            if (prefab.TryGetComponent(out ProjectileEffect projectileEff))
            {
                projectileEff.Fire();
            }
        }
    }
    public void PlayBuffParticle()
    {
        Instantiate(currentSkillEffect, transform);
    }
    private IEnumerator SkillCoolTime(ActiveSkill skill, int index)
    {
        skill.CurrentCooldown = skill.ActiveData.cooldown;
        UIManager.Instance.ActiveCoolDownImage(index);

        while(skill.CurrentCooldown > 0f)
        {
            skill.CurrentCooldown -= Time.deltaTime;
            skill.CurrentCooldown = Mathf.Max(0f, skill.CurrentCooldown);

            float ratio = skill.CurrentCooldown / skill.ActiveData.cooldown;
            UIManager.Instance.CoolDownImage(index, ratio);

            yield return null;
        }

        UIManager.Instance.CoolDownImage(index, 0f);
    }
}
