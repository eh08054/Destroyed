using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    public GameObject soldOutpanel;
    public Button button;
    public bool isSoldOut;

    private ItemData _itemData;
    public int slotIndex;

    public event Action<ItemData> OnHoverEnter;
    public event Action OnHoverExit;
    private bool isHovered = false;

    private void Awake()
    {
        isSoldOut = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (_itemData != null) OnHoverEnter?.Invoke(_itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        OnHoverExit?.Invoke();
    }

    private void OnDisable()
    {
        if (isHovered)
        {
            isHovered = false;
            OnHoverExit?.Invoke();
        }
    }

    public void SetItem(ItemData itemData, int price)
    {
        _itemData = itemData;
        itemImage.sprite = itemData.ItemIcon;
        itemImage.color = Color.white;
        itemNameText.text = itemData.itemName;
        priceText.text = price.ToString();
    }
}
