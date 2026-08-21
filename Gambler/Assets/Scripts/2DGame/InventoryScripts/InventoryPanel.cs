using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Button sortButton;
    [SerializeField] private DupCheckPanel dupCheckPanel;

    public List<InventorySlot> slots;

    public void Start()
    {
        sortButton.onClick.AddListener(() => GameManager.Instance.inventoryController.SortInventory());
        gameObject.SetActive(false);
    }
    public void InitializeUI(Inventory inventory)
    {
        for (int i = 0; i < inventory.maxSlotCount; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotParent);
            InventorySlot slot = slotObject.GetComponent<InventorySlot>();
            slots.Add(slot);

        }
        GameManager.Instance.inventoryController.SetSlots(slots);
        GameManager.Instance.inventoryController.SetDupcheckPanel(dupCheckPanel);
    }
}
