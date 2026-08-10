using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public string saveName;
    public int value;
    public string description;
    public float durationTime;
    public GameObject _itemPrefab;
    public Sprite ItemIcon;
    public int maxStorageStack;
    public ItemType itemType;
}
public enum ItemType
{
    Potion,
    Sword,
    Shield,
    Gold,
}
