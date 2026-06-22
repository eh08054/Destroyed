using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private ItemData _item;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_item.itemType == ItemData.ItemType.Gold)
            {
                GameManager.Instance.AddGold(GetComponent<GoldDrop>().amount);
                Destroy(gameObject);
            }
            else
            {
                Transform inventory = GameManager.Instance.InventoryPanel.transform;
                bool check = inventory.GetComponentInChildren<InventoryController>().AddItem(_item);
                if (check == true)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
