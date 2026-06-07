using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
   public Inventory Inventory { get; private set; }
    [SerializeField] private Transform SlotParent;
    private void Awake()
    {
        Inventory = new Inventory
        {
            Slots = SlotParent.GetComponentsInChildren<Slot>(),
            itemSlots = new List<ItemSlot>()
        };
        GameManager.Instance.RegisterInventory(Inventory);
    }
    private void Start()
    {
        FreshSlot();
        GameManager.Input.keyAction -= InventoryControl;
        GameManager.Input.keyAction += InventoryControl;
        gameObject.SetActive(false);
    }
    public void InventoryControl()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
    public void FreshSlot()
    {
        int i = 0;
        for (; i < Inventory.itemSlots.Count && i < Inventory.Slots.Length; i++)
        {
            Inventory.Slots[i].ItemSlot = Inventory.itemSlots[i];
        }
        for (; i < Inventory.Slots.Length; i++)
        {
            Inventory.Slots[i].ItemSlot = null;
        }
        for(int j = 0; j < Inventory.Slots.Length; j++)
        {
            Inventory.Slots[j].slotIndex = j;
        }
    }
    public bool AddItem(ItemData _item)
    {
        bool check = Inventory.AddItem(_item);
        if (check == true)
        {
            FreshSlot();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void UseItem(ItemData _item)
    {
        PlayerController player = GameManager.Instance.Player.GetComponent<PlayerController>();
        switch (_item.itemType)
        {
            case ItemData.ItemType.Potion:
                player.HealPlayer(_item.value);
                break;
            case ItemData.ItemType.Sword:
                break;
            case ItemData.ItemType.Shield:
                break;
        }
    }
    public void RemoveItem(int slotIndex)
    {
        Inventory.RemoveItem(slotIndex);
        FreshSlot();
    }
}
