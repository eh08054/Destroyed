using UnityEngine;

[CreateAssetMenu(fileName = "PotionData", menuName = "Scriptable Objects/PotionData")]
public class PotionData : ItemData
{
    public PotionType potionType;

    public void ApplyEffect(PlayerController player, PotionType type)
    {
        switch (type)
        {
            case PotionType.Heal:
                player.HealPlayer(value);
                break;
            case PotionType.Attack:
                player.AttackPotionOn(value, durationTime);
                break;
        }
    }
    public void ReleaseEffect(PlayerController player, PotionType type)
    {
        switch (type)
        {
            case PotionType.Attack:
                player.AttackPotionOff(value);
                break;
        }
    }
}
public enum PotionType
{
    Heal,
    Attack,
    Speed,
    Health,
}