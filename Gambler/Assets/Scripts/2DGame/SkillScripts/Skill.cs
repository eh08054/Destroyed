using UnityEngine;

public abstract class Skill
{
    public SkillData skillData { get; protected set; }
    public int level;
    public float sumValue;
}
