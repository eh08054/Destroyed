using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemSlot
{
    public ItemData item;
    public int count;
    public int maxStack;
    public bool CanStack => count < maxStack;
}

public class Inventory
{
    public event Action OnInventoryChanged;
    public List<ItemSlot> itemSlots;
    public int maxSlotCount;
    public bool AddItem(ItemData _item)
    {
        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.item == _item && itemSlot.CanStack)
            {
                itemSlot.count++;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        if (itemSlots.Count < maxSlotCount)
        {
            itemSlots.Add(new ItemSlot { item = _item, count = 1, maxStack = _item.maxStorageStack });
            OnInventoryChanged?.Invoke();
            return true;
        }

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
        OnInventoryChanged?.Invoke();
    }

    public void Sort()
    {
        itemSlots.Sort((a, b) => a.item.id.CompareTo(b.item.id));
        OnInventoryChanged?.Invoke();
    }
}
