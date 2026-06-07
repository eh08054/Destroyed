using UnityEngine;
using System.Collections.Generic;
public abstract class PlayerBase
{
    public abstract string PlayerName { get; }
    public abstract int MaxHP { get; }
    public int CurrentHP { get; set; }
    public abstract float AttackRange { get;}
    public abstract int AttackDamage { get; set; }
    public abstract float AttackCoolTime { get; }
    public enum AttackType { Jab, Slash };
    public enum State { Idle, Attack, Dead}
    public State CurrentState { get; set; } = State.Idle;
    public WeaponData currentWeapon { get; private set; }
    public List<WeaponData> ownedWeapons { get; private set; } = new List<WeaponData>();
    public List<SkillData> skillDatas;
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
        switch (skillData.skillType)
        {
            case SkillData.SkillType.AttackUp:
                AttackDamage += (int)skillData.valuePerSkill[skillData.skillLevel];
                skillData.skillLevel++;
                break;
        }
    }
}
