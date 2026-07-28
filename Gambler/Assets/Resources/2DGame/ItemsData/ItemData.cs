using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
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
