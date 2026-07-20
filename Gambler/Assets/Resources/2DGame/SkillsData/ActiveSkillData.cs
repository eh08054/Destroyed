using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillData", menuName = "Scriptable Objects/ActiveSkillData")]
public class ActiveSkillData : SkillData
{
    public enum ActiveSkillType { Original, Enhance}
    public enum ActiveType
    {
        projectile,
        Roll,
        Buff,
    }
    public enum EnhanceType
    {
        CoolDownReduce,
        RangeIncrease,
        DamageIncrease,
    }

    public enum BuffType
    {
        AttackUp,
    }

    [Header("Skill Type")]
    public ActiveSkillType activeSkillType;

    [Header("Original")]
    public GameObject skillEffectPrefab;
    public float cooldown;
    public ActiveType activeType;

    [Header("Enhance")]
    public ActiveSkillData targetSkill;
    public ActiveSkillData requiredSkill;
    public EnhanceType enhanceType;

    [Header("Buff")]
    public float duration;
    public BuffType buffType;
    
    
}
