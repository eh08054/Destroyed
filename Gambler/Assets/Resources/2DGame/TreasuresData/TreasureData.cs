using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TreasureData", menuName = "Scriptable Objects/TreasureData")]
public class TreasureData : ScriptableObject
{
    public enum TreasureType
    {
        Wooden,
        Silver,
        Golden,
    }
    public TreasureType treasureType;

    public int gold;
    public List<ItemData> items;
}
