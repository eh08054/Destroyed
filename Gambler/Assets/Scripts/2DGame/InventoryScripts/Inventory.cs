using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSlot
{
    public ItemData item;
    public int count;
    public int maxStack;
    public bool CanStack => count < maxStack;
}

public class Inventory
{
    public List<ItemSlot> itemSlots;
    public Slot[] Slots { get; set; }
    public bool AddItem(ItemData _item)
    {
        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.item == _item && itemSlot.CanStack)
            {
                itemSlot.count++;
                return true;
            }
        }
        if (itemSlots.Count < Slots.Length)
        {
            itemSlots.Add(new ItemSlot { item = _item, count = 1, maxStack = _item.maxStorageStack });
            return true;
        }

        Debug.Log("½½·ÔÀÌ °¡µæ Â÷ ÀÖ½À´Ï´Ù.");
        return false;
    }
    public void RemoveItem(int slotIndex)
    {
        if(slotIndex >= itemSlots.Count) { return; }

        itemSlots[slotIndex].count--;
        if (itemSlots[slotIndex].count <= 0)
        {
            itemSlots.RemoveAt(slotIndex);
        }      
    }
}
