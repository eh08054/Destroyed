using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryController : MonoBehaviour
{
    public Inventory Inventory { get; private set; }
    public HashSet<ItemData> ActiveItems { get; private set; }

    private ItemToolTip itemToolTip;
    private List<InventorySlot> slots;
    private DupCheckPanel dupCheckPanel;
    private void Awake()
    {
        ActiveItems = new HashSet<ItemData>();
    }
    public void InitializeInventory(Inventory inventory)
    {
        Inventory = inventory;
        Inventory.OnInventoryChanged += HandleInventoryChanged;
    }
    public void OnDestroy()
    {
        if(Inventory != null)
        {
            Inventory.OnInventoryChanged -= HandleInventoryChanged;
        }
    }
    private void HandleInventoryChanged()
    {
        FreshSlot(slots);
    }
    public void SetSlots(List<InventorySlot> slots)
    {
        this.slots = slots;
        FreshSlot(slots);
    }
    public void SetDupcheckPanel(DupCheckPanel panel)
    {
        dupCheckPanel = panel;
    }

    public void FreshSlot(List<InventorySlot> slots)
    {
        itemToolTip.gameObject.SetActive(false);
        int i = 0;
        for (; i < Inventory.itemSlots.Count && i < Inventory.maxSlotCount; i++)
        {
            slots[i].ItemSlot = Inventory.itemSlots[i];
            slots[i].OnHoverEnter -= ActivateToolTip;
            slots[i].OnHoverExit -= DeActivateToopTip;

            slots[i].OnHoverEnter += ActivateToolTip;
            slots[i].OnHoverExit += DeActivateToopTip;
        }
        for (; i < Inventory.maxSlotCount; i++)
        {
            slots[i].ItemSlot = null;
            slots[i].OnHoverEnter -= ActivateToolTip;
            slots[i].OnHoverExit -= DeActivateToopTip;
        }
        for (int j = 0; j < Inventory.maxSlotCount; j++)
        {
            slots[j].slotIndex = j;
        }
    }
    public bool CheckFull(ItemData itemData)
    {
        for(int i = 0; i < Inventory.maxSlotCount; i++)
        {
            if (slots[i].ItemSlot == null)
            {
                return false;
            }
            else if (slots[i].ItemSlot.item == itemData && slots[i].ItemSlot.CanStack)
            {
                return false;
            }
        }
        return true;
    }
    public void ClickedItem(ItemData _item, int slotIndex)
    {
        if(_item == null) { return; }
        UseItem(_item, () => RemoveItem(slotIndex));
    }
    public void AddItem(ItemData _item, GameObject gameObject = null)
    {
        if (Inventory.AddItem(_item)) { Destroy(gameObject); }
    }
    public void UseItem(ItemData _item, Action onUsed)
    {
        PlayerController player = GameManager.Instance.Player.GetComponent<PlayerController>();
        switch (_item.itemType)
        {
            case ItemType.Potion:
                UsePotion((PotionData)_item, player, onUsed);
                break;
            case ItemType.Sword:
                break;
            case ItemType.Shield:
                break;
        }
    }

    public void UsePotion(PotionData potion, PlayerController player, Action onUsed)
    {
        if (potion.potionType != PotionType.Heal && ActiveItems.Contains(potion))
        {
            dupCheckPanel.Show(
                onYes: () =>
                {
                    ReleaseItemBuff(player, potion);
                    ApplyPotionEffect(potion, player);
                    onUsed?.Invoke();
                },
                onNo: () => { }
                );
            return;
        }

        ApplyPotionEffect(potion, player);
        onUsed.Invoke();
    }
    public void ApplyPotionEffect(PotionData potion, PlayerController player)
    {
        potion.ApplyEffect(player, potion.potionType);
        if (potion.potionType != PotionType.Heal)
        {
            player.AttachPotionEffect();
            ActiveItems.Add(potion);
            UIManager.Instance.ResisterItem(potion, () => ReleaseItemBuff(player, potion));
        }
    }
    public void RemoveItem(int slotIndex)
    {
        Inventory.RemoveItem(slotIndex);
    }

    public void ReleaseItemBuff(PlayerController player, ItemData item)
    {
        if (item is PotionData potion)
        {
            potion.ReleaseEffect(player, potion.potionType);
            player.DetachPotionEffect();
        }
        ActiveItems.Remove(item);
        UIManager.Instance.RemoveItem(item);
    }

    public void SetItemToolTip(ItemToolTip itemToolTip)
    {
        this.itemToolTip = itemToolTip;
    }
    public void ActivateToolTip(ItemData itemData)
    {
        itemToolTip.SetPanel(itemData.itemName, itemData.ItemIcon, itemData.description);
        UIManager.Instance.ToolTipPanel.SetActive(true);
    }
    public void SortInventory()
    {
        Inventory.Sort();
    }
    public void DeActivateToopTip()
    {
        UIManager.Instance.ToolTipPanel.SetActive(false);
    }
}
