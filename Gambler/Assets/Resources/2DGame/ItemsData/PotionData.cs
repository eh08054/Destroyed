using UnityEngine;

[CreateAssetMenu(fileName = "PotionData", menuName = "Scriptable Objects/PotionData")]
public class PotionData : ItemData
{
    public PotionType potionType;
}
public enum PotionType
{
    Heal,
    Attack,
    Speed,
    Health,
}