using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillData", menuName = "Scriptable Objects/ActiveSkillData")]
public class ActiveSkillData : SkillData
{
    public GameObject skillEffectPrefab;
    public float cooldown;
    public enum AttackType
    {
        projectile,
    }
    public AttackType attackType;
}
