using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillData", menuName = "Scriptable Objects/ActiveSkillData")]
public class ActiveSkillData : SkillData
{
    public enum AttackSkillType { Original, Enhance}
    public enum AttackType
    {
        projectile,
    }
    public enum EnhanceType
    {
        CoolDownReduce,
        RangeIncrease,
        DamageIncrease,
    }

    [Header("Skill Type")]
    public AttackSkillType attackSkillType;

    [Header("Original")]
    public GameObject skillEffectPrefab;
    public float cooldown;
    public AttackType attackType;

    [Header("Enhance")]
    public ActiveSkillData targetSkill;
    public ActiveSkillData requiredSkill;
    public EnhanceType enhanceType;
    
    
}
