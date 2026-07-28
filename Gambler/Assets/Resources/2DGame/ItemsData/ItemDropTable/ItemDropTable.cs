using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDropTable", menuName = "Scriptable Objects/ItemDropTable")]
public class ItemDropTable : ScriptableObject
{
    [System.Serializable]
    public class DropItem
    {
        public ItemData _item;
        public int weight;
    }

    [Header("필연 재화")]
    public int goldMin;
    public int goldMax;
    public ItemData goldData;
    public int expAmount;

    [Header("확률 아이템")]
    public List<DropItem> dropItems = new List<DropItem>();
}
