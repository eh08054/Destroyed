using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public int id;
    public string skillName;
    public Sprite skillIcon;
    public int skillLevel;
    public int skillMaxLevel;
    public float[] valuePerSkill;
    public int[] goldPerLevel;
    public enum SkillType
    {
        AttackUp,
        SpeedUp,
        DefendUp,
        HPUp,
    }
    public SkillType skillType;
}
