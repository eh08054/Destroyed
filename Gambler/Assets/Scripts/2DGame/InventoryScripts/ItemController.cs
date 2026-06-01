using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private Item _item;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform inventory = GameManager.Instance.Inventory.transform.GetChild(0);
            bool check = inventory.GetComponent<InventoryController>().AddItem(_item);
            if (check == true)
            {
                Destroy(gameObject);
            }
        }
    }
}
