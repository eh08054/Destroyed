using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDropTable", menuName = "Scriptable Objects/ItemDropTable")]
public class ItemDropTable : ScriptableObject
{
    [System.Serializable]
    public class DropItem
    {
        public Item _item;
        public int weight;
    }

    [Header("필연 재화")]
    [SerializeField] private int goldMin;
    [SerializeField] private int goldMax;
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private int expAmount;

    [Header("확률 아이템")]
    [SerializeField] private List<DropItem> dropItems = new List<DropItem>();

    protected Item PickItem()
    {
        int sum = 0;
        foreach (var dropItem in dropItems)
        {
            sum += dropItem.weight;
        }
        int rnd = Random.Range(0, sum);

        for (int i = 0; i < dropItems.Count; i++)
        {
            DropItem dropItem = dropItems[i];
            if (dropItem.weight >= rnd)
            {
                return dropItems[i]._item;
            }
            else
            {
                rnd -= dropItem.weight;
            }
        }
        return null;
    }
    public void ItemDrop(Vector3 pos)
    {
        DropGold(pos);
        Item pickItem = PickItem();
        if (pickItem != null)
        {
            Instantiate(pickItem._itemPrefab, pos, Quaternion.identity);
        }
    }
    public void DropGold(Vector3 pos)
    {
        int amount = Random.Range(goldMin, goldMax);
        GameObject goldObj = Instantiate(goldPrefab, pos + Vector3.right * 0.2f, Quaternion.identity);
        goldObj.GetComponent<GoldDrop>().amount = amount;
    }
}
