using UnityEngine;

public class PassiveSkill : Skill
{
    public PassiveSkillData PassiveData => skillData as PassiveSkillData;
    public PassiveSkillData.SkillType skillType;
    public PassiveSkill(PassiveSkillData passiveSkillData)
    {
        skillData = passiveSkillData;
        level = 0;
        sumValue = 0;
        skillType = passiveSkillData.skillType;
    }
}
