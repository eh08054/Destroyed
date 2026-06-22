using UnityEngine;
public abstract class SkillData : ScriptableObject
{
    public int id;
    public string skillName;
    public Sprite skillIcon;
    public int skillLevel;
    public int skillMaxLevel;
    public float sumValues;
    public float[] valuePerLevel;
    public int[] goldPerLevel;

    [TextArea(2, 5)] public string descriptionFormat;
    [TextArea(2, 5)] public string skillDescriptionFormat;
}
