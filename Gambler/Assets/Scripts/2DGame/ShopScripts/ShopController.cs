using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public Shop Shop { get; private set; }
    private List<ShopSlot> slots;
    [SerializeField] private ShopPanel shopPanel;
    [SerializeField] private ShopDescriptionPanel descPanel;
    [SerializeField] private PurchaseConfirmPanel confPanel;
    [SerializeField] private List<ItemData> tempItems;

    void Awake()
    {
        Shop = GenerateRandomShop();
    }
    private void Start()
    {
        shopPanel.InitializeUI();
    }

    private Shop GenerateRandomShop()
    {
        var shop = new Shop { shopItems = new List<ShopItem>() };
        for (int i = 0; i < 10; i++)
        {
            ItemData tempItem = tempItems[Random.Range(0, 3)];
            shop.shopItems.Add(new ShopItem { item = tempItem, price = 10 });
        }
        shop.OnShopChanged += HandleShopChanged;
        return shop;
    }

    private void HandleShopChanged()
    {
        FreshSlot();
    }

    public void SetSlots(List<ShopSlot> slots)
    {
        this.slots = slots;
        FreshSlot();
    }
    public void FreshSlot()
    {
        for (int i = 0; i < slots.Count && i < Shop.shopItems.Count; i++)
        {
            var shopItem = Shop.shopItems[i];
            var slot = slots[i];

            slot.SetItem(shopItem.item, shopItem.price);
            slot.OnHoverEnter -= ActiveDescPanel;
            slot.OnHoverExit -= DeActivateDescPanel;

            slot.OnHoverEnter += ActiveDescPanel;
            slot.OnHoverExit += DeActivateDescPanel;

            if (!slot.isSoldOut)
            {
                slot.button.onClick.RemoveAllListeners();
                slot.button.onClick.AddListener(() => ShowConfirmPanel(slot.slotIndex));
            }
        }
    }

    public void ShowConfirmPanel(int slotIndex)
    {
        confPanel.confirmButton.onClick.RemoveAllListeners();
        confPanel.cancleButton.onClick.RemoveAllListeners();

        confPanel.confirmButton.onClick.AddListener(() => BuyItem(slotIndex));
        confPanel.cancleButton.onClick.AddListener(() => confPanel.gameObject.SetActive(false));

        confPanel.text.text = Shop.shopItems[slotIndex].item.itemName + " 을 구입하시겠습니까?";
        confPanel.gameObject.SetActive(true);
    }

    public void BuyItem(int slotIndex)
    {
        confPanel.gameObject.SetActive(false);

        if (GameManager.Instance.inventoryController.CheckFull(Shop.shopItems[slotIndex].item)) 
        {
            Debug.Log("FULL INVENTORY");
            return; 
        }
        int playerGold = GameManager.Instance.GameData.Gold;

        if (Shop.TryPurchase(slotIndex, playerGold, out int cost))
        {
            GameManager.Instance.UseGold(cost);
            GameManager.Instance.inventoryController.AddItem(Shop.shopItems[slotIndex].item);
            shopPanel.RefreshCash();
            slots[slotIndex].soldOutpanel.SetActive(true);
            slots[slotIndex].isSoldOut = true;
            slots[slotIndex].button.onClick.RemoveAllListeners();
        }
        else
        {
            // 구매 실패 UI 피드백 (골드 부족 등)
        }
    }
    public void Restock()
    {
        foreach (var slot in slots)
        {
            if (slot.isSoldOut)
            {
                slot.isSoldOut = false;
                slot.soldOutpanel.SetActive(false);
            }
        }
        Shop.shopItems.Clear();
        Shop = GenerateRandomShop();
        FreshSlot();
    }
    public void ActiveDescPanel(ItemData item)
    {
        descPanel.SetDescriptionPanel(item.ItemIcon, item.itemName, item.description, 10);
        descPanel.gameObject.SetActive(true);
    }
    public void DeActivateDescPanel()
    {
        descPanel.gameObject.SetActive(false);
    }

}
