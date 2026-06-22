using UnityEngine;

public class ActiveSkill : Skill
{
    public ActiveSkillData ActiveData => skillData as ActiveSkillData; 
    public float MaxCoolDown { get; set; }
    public float CurrentCooldown { get; set; }
    public bool IsReady => CurrentCooldown <= 0f;
    public ActiveSkill(ActiveSkillData activeSkillData)
    {
        skillData = activeSkillData;
        level = 0;
        sumValue = 0;
        CurrentCooldown = 0f;
        MaxCoolDown = ActiveData.cooldown;
    }
}
