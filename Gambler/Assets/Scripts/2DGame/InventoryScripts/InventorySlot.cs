using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemCountText;
    [SerializeField] private Button button;
    private ItemSlot _itemSlot;
    public int slotIndex;

    public event Action<ItemData> OnHoverEnter;
    public event Action OnHoverExit;
    private bool isHovered = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        OnHoverEnter?.Invoke(_itemSlot.item);
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
    private void OnDestroy()
    {
        button.onClick.RemoveListener(() => GameManager.Instance.inventoryController.ClickedItem(_itemSlot.item, slotIndex));
    }
    public ItemSlot ItemSlot
    {
        get { return _itemSlot; }
        set
        {
            _itemSlot = value;
            if(_itemSlot != null)
            {
                itemImage.sprite = _itemSlot.item.ItemIcon;
                itemImage.color = new Color(1, 1, 1, 1);
                //itemText.text = _itemSlot.item.itemName;
                itemCountText.text = _itemSlot.count.ToString();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => GameManager.Instance.inventoryController.ClickedItem(_itemSlot.item, slotIndex));
            }
            else
            {
                itemImage.color = new Color(1, 1, 1, 0);
                //itemText.text = "";
                itemCountText.text = "";
            }
        }
    }
}
