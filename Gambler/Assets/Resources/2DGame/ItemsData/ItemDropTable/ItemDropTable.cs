using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDropTable", menuName = "Scriptable Objects/ItemDropTable")]
public class ItemDropTable : ScriptableObject
{
    [System.Serializable]
    public class Item
    {
        public ItemData item;
        public int weight;
    }

    [Header("필연 재화")]
    [SerializeField] private int goldMin;
    [SerializeField] private int goldMax;
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private int expAmount;

    [Header("확률 아이템")]
    [SerializeField] private List<Item> items = new List<Item>();

    protected ItemData PickItem()
    {
        int sum = 0;
        foreach (var item in items)
        {
            sum += item.weight;
        }
        int rnd = Random.Range(0, sum);

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            if (item.weight >= rnd)
            {
                return items[i].item;
            }
            else
            {
                rnd -= item.weight;
            }
        }
        return null;
    }
    public void ItemDrop(Vector3 pos)
    {
        DropGold(pos);
        ItemData item = PickItem();
        Instantiate(item, pos, Quaternion.identity);
    }
    public void DropGold(Vector3 pos)
    {
        int amount = Random.Range(goldMin, goldMax);
        GameObject goldObj = Instantiate(goldPrefab, pos, Quaternion.identity);
        goldObj.GetComponent<GoldDrop>().amount = amount;
    }
}
