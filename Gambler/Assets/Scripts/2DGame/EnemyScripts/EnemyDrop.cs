using UnityEngine;
using static ItemDropTable;

public class EnemyDrop : MonoBehaviour
{
    public void DropItem(ItemDropTable dropTable, Vector3 pos)
    {
        DropGold(dropTable, pos);
        ItemData pickItem = PickItem(dropTable, pos);

        if (pickItem != null)
        {
            GameObject newItem = Instantiate(pickItem._itemPrefab, pos, Quaternion.identity);
            newItem.GetComponent<SpriteRenderer>().sprite = pickItem.ItemIcon;
            newItem.GetComponent<ItemController>()._item = pickItem;
        }
    }
    public void DropGold(ItemDropTable dropTable, Vector3 pos)
    {
        int amount = Random.Range(dropTable.goldMin, dropTable.goldMax);
        GameObject goldObj = Instantiate(dropTable.goldData._itemPrefab, pos + Vector3.right * 0.2f, Quaternion.identity);
        goldObj.GetComponent<GoldDrop>().amount = amount;
    }
    public ItemData PickItem(ItemDropTable dropTable, Vector3 pos)
    {
        int sum = 0;
        foreach (var dropItem in dropTable.dropItems)
        {
            sum += dropItem.weight;
        }
        int rnd = Random.Range(0, sum);

        for (int i = 0; i < dropTable.dropItems.Count; i++)
        {
            DropItem dropItem = dropTable.dropItems[i];
            if (dropItem.weight >= rnd)
            {
                return dropTable.dropItems[i]._item;
            }
            else
            {
                rnd -= dropItem.weight;
            }
        }
        return null;
    }
}
