using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public int AttackDamage;
    public SFX Attack_normal;
    public SFX Attack_Hit;
    public float AttackRange;
}
