using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillData", menuName = "Scriptable Objects/PassiveSkillData")]
public class PassiveSkillData : SkillData
{
    public enum SkillType
    {
        AttackUp,
        SpeedUp,
        DefendUp,
        HPUp,
    }
    public SkillType skillType;
}
