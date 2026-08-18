using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private ShopController shopController;
    [SerializeField] private TMP_Text cashText;
    public List<ShopSlot> slots;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void InitializeUI()
    {
        for (int i = 0; i < shopController.Shop.shopItems.Count; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotParent);
            ShopSlot slot = slotObject.GetComponent<ShopSlot>();
            slot.slotIndex = i;
            slots.Add(slot);
        }
        shopController.SetSlots(slots);
        cashText.text = GameManager.Instance.GameData.Gold.ToString();
    }
    public void RefreshCash()
    {
        cashText.text = GameManager.Instance.GameData.Gold.ToString();
    }
}
