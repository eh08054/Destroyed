using UnityEngine;
using UnityEngine.UI;
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
    public int weaponDamage;
    public WeaponType weaponType;
    public Sprite weaponIcon;
}
