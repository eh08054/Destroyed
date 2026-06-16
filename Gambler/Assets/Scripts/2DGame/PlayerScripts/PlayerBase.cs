using Assets.PixelFantasy.PixelTileEngine.Scripts;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerBase
{
    public abstract string PlayerName { get; }
    public abstract int MaxHP { get; set; }
    public int CurrentHP { get; set; }
    public abstract float AttackRange { get;}
    public abstract int AttackDamage { get; set; }
    public abstract float AttackCoolTime { get; }
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
        Skill targetSkill;

        if(ownedSkills.TryGetValue(skillData, out Skill existingSkill))
        {
            if(existingSkill.level == existingSkill.skillData.skillMaxLevel) { return; }
            if (GameManager.Instance.GameData.gold < skillData.goldPerLevel[existingSkill.level])
            {
                Debug.Log("골드가 부족합니다.");
                return;
            }
            GameManager.Instance.UseGold(skillData.goldPerLevel[skillData.skillLevel]);

            LevelUp(existingSkill);
            targetSkill = existingSkill;
        }
        else
        {
            Skill newSkill = skillData is ActiveSkillData activeData ? new ActiveSkill(activeData) :
                new PassiveSkill((PassiveSkillData)skillData);

            if (GameManager.Instance.GameData.gold < skillData.goldPerLevel[newSkill.level])
            {
                Debug.Log("골드가 부족합니다.");
                return;
            }
            GameManager.Instance.UseGold(skillData.goldPerLevel[newSkill.level]);

            LevelUp(newSkill);
            ownedSkills.Add(skillData, newSkill);

            if (newSkill is ActiveSkill activeSkill)
            {
                GameManager.Instance.Player.GetComponent<SkillController>().EquipSkill(0, activeSkill);
            }
            targetSkill = newSkill;
        }

        if(skillData is PassiveSkillData passiveData)
        {
            ApplyPassiveStat(passiveData, targetSkill);
        }
    }

    public void ApplyPassiveStat(PassiveSkillData passiveData, Skill skill)
    {
            switch (passiveData.skillType)
            {
                case PassiveSkillData.SkillType.AttackUp:
                    AttackDamage += (int)passiveData.valuePerLevel[skill.level];
                    break;
                case PassiveSkillData.SkillType.HPUp:
                    MaxHP += (int)passiveData.valuePerLevel[skill.level];
                    CurrentHP += (int)passiveData.valuePerLevel[skill.level];
                    GameManager.Instance.ChangeHP();
                    break;
            }
    }
    public void LevelUp(Skill skill)
    {
        skill.level++;
        skill.sumValue += skill.skillData.valuePerLevel[skill.level];
    }
}
