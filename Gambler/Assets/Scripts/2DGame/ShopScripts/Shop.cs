using System;
using System.Collections.Generic;

public class ShopItem
{
    public ItemData item;
    public int price;
    //public int stock;
}

public class Shop
{
    public event Action OnShopChanged;
    public List<ShopItem> shopItems;

    public bool TryPurchase(int slotIndex, int playerGold, out int cost)
    {
        cost = 0;
        if (slotIndex >= shopItems.Count) return false;

        var shopItem = shopItems[slotIndex];
        cost = shopItem.price;

        if (playerGold < cost) return false;
        //if (shopItem.stock <= 0) return false; 

        //shopItem.stock--;
        OnShopChanged?.Invoke();
        return true;
    }
}
