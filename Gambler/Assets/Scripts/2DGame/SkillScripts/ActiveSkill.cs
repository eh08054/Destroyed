using UnityEngine;

public class ActiveSkill : Skill
{
    public ActiveSkillData ActiveData => skillData as ActiveSkillData; 
    public float MaxCoolDown { get; set; }
    public float CurrentCooldown { get; set; }
    public float Duration { get; set; }
    public bool IsReady => CurrentCooldown <= 0f;
    public ActiveSkill(ActiveSkillData activeSkillData)
    {
        skillData = activeSkillData;
        level = 0;
        sumValue = 0;
        CurrentCooldown = 0f;
        MaxCoolDown = ActiveData.cooldown;
        Duration = ActiveData.duration;
    }

    public void ApplyBuff(PlayerBase player)
    {
        switch (ActiveData.buffType)
        {
            case ActiveSkillData.BuffType.AttackUp:
                player.AttackBuffOn(ActiveData.sumValues);
                break;
            default:
                break;
        }
    }

    public void ReleaseBuff(PlayerBase player)
    {
        switch (ActiveData.buffType)
        {
            case ActiveSkillData.BuffType.AttackUp:
                player.AttackBuffOff(ActiveData.sumValues);
                break;
            default:
                break;
        }
    }


}
