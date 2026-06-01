using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public AnimatorOverrideController animatorOverride;
    public enum WeaponType
    {
        Sword,
        Gun,
        Bow,
    }
    public WeaponType weaponType;
}
