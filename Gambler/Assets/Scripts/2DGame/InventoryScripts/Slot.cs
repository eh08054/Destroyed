using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Slot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private TMP_Text itemCountText;
    private ItemSlot _itemSlot;
    private Button button;
    public int slotIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void Start()
    {
        button.onClick.AddListener(ClickedItem);
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
                itemText.text = _itemSlot.item.itemName;
                itemCountText.text = _itemSlot.count.ToString();
            }
            else
            {
                itemImage.color = new Color(1, 1, 1, 0);
                itemText.text = "";
                itemCountText.text = "";
            }
        }
    }
    
    private void ClickedItem()
    {
        UseItem();
        RemoveItem();
    }

    private void UseItem()
    {
        GameManager.Instance.Inventory.GetComponentInChildren<InventoryController>().UseItem(_itemSlot.item);
    }
    private void RemoveItem()
    {
        GameManager.Instance.Inventory.GetComponentInChildren<InventoryController>().RemoveItem(slotIndex);
    }

}
