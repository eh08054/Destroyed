using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemData _item;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_item.itemType == ItemType.Gold)
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
