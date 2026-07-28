using Assets.PixelFantasy.PixelTileEngine.Scripts;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerBase
{
    public abstract string PlayerName { get; }
    public abstract int MaxHP { get; set; }
    public int CurrentHP { get; set; }
    public abstract float AttackRange { get;}
    public abstract int ATK { get; set; }
    public abstract int DEF { get; set; }
    public abstract float AttackCoolTime { get; }
    public float MoveSpeed { get; set; }
    public enum AttackType { Jab, Slash };
    public enum State { Idle, Attack, Dead}
    public State CurrentState { get; set; } = State.Idle;
    public WeaponData currentWeapon { get; private set; }
    public List<WeaponData> ownedWeapons { get; private set; } = new();
    public Dictionary<SkillData, Skill> ownedSkills { get; private set; } = new();
    public int currentWeaponIndex;
    public virtual void Init()
    {
        CurrentHP = MaxHP;
        CurrentState = State.Idle;
        currentWeaponIndex = 0;
    }
    public virtual void TakeDamage(int damage)
    {
        CurrentHP -= damage;
    }
    public void HealPlayer(int amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, 0, MaxHP);
    }
    public void AddWeapon(WeaponData weapon)
    {
        if (!ownedWeapons.Contains(weapon))
        {
            ownedWeapons.Add(weapon);
        }
    }
    public void ChangeWeapon(WeaponData weapon)
    {
        if(ownedWeapons.Contains(weapon))
        {
            currentWeapon = weapon;
        }
    }
    public void ApplySkillEffect(SkillData skillData)
    {
        if(ownedSkills.TryGetValue(skillData, out Skill existingSkill))
        {
            if(existingSkill.level == existingSkill.skillData.skillMaxLevel) { return; }
            if (GameManager.Instance.GameData.Gold < skillData.goldPerLevel[existingSkill.level])
            {
                Debug.Log("골드가 부족합니다.");
                return;
            }
            LevelUp(existingSkill);
        }
        else
        {
            Skill newSkill = skillData is ActiveSkillData activeData ? new ActiveSkill(activeData) :
                new PassiveSkill((PassiveSkillData)skillData);

            if (GameManager.Instance.GameData.Gold < skillData.goldPerLevel[newSkill.level])
            {
                Debug.Log("골드가 부족합니다.");
                return;
            }
            LevelUp(newSkill);
            ownedSkills.Add(skillData, newSkill);

            if (newSkill is ActiveSkill activeSkill 
                && activeSkill.ActiveData.activeSkillType == ActiveSkillData.ActiveSkillType.Original)
            {
                GameManager.Instance.Player.GetComponent<SkillController>().EquipSkill(activeSkill);
            }
        }
    }
    public void LevelUp(Skill skill)
    {
        if (skill is PassiveSkill passiveSkill)
        {
            ApplyPassiveStat(passiveSkill);
        }
        else if(skill is ActiveSkill activeSkill 
            && activeSkill.ActiveData.activeSkillType == ActiveSkillData.ActiveSkillType.Enhance)
        {
            ApplyActiveStat(activeSkill);
        }

        GameManager.Instance.UseGold(skill.skillData.goldPerLevel[skill.level]);
        skill.sumValue += skill.skillData.valuePerLevel[skill.level];
        skill.level++;
    }
    public void ApplyPassiveStat(PassiveSkill skill)
    {
        switch (skill.skillType)
        {
            case PassiveSkillData.SkillType.AttackUp:
                ATK += (int)skill.skillData.valuePerLevel[skill.level];
                break;
            case PassiveSkillData.SkillType.HPUp:
                MaxHP += (int)skill.skillData.valuePerLevel[skill.level];
                CurrentHP += (int)skill.skillData.valuePerLevel[skill.level];
                GameManager.Instance.ChangeHP();
                break;
            case PassiveSkillData.SkillType.DefenseUp:
                DEF += (int)skill.skillData.valuePerLevel[skill.level];
                break;
        }
    }
    public void ApplyActiveStat(ActiveSkill skill)
    {
        if(!ownedSkills.TryGetValue(skill.ActiveData.targetSkill, out Skill targetSkill)) { return; }
        if(targetSkill is not ActiveSkill activeSkill) { return; }
        switch (skill.ActiveData.enhanceType)
        {
            case ActiveSkillData.EnhanceType.CoolDownReduce:
                activeSkill.MaxCoolDown -= (skill.skillData.valuePerLevel[skill.level] / 100);
                break;
        }
    }

    public void AttackBuffOn(float attackPlus)
    {
        ATK += (int)attackPlus;
    }
    public void AttackBuffOff(float attackMinus)
    {
        ATK -= (int)attackMinus;
    }
    public void SpeedBuffOn(float speedPlus)
    {
        MoveSpeed += speedPlus;
    }
    public void SpeedBuffOff(float speedMinus)
    {
        MoveSpeed -= speedMinus;
    }
}
