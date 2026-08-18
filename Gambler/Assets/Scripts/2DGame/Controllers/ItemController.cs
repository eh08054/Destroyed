using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour
{
    public ItemData _item;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 landingPoint;

    private void Start()
    {
        landingPoint = CalculateLandingPoint();
        StartCoroutine(DropAnimation());
    }

    private Vector3 CalculateLandingPoint()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, groundLayer);
        if(hit.collider != null)
        {
            return new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
        return transform.position + Vector3.down * 1f;
    }

    private IEnumerator DropAnimation()
    {
        Vector3 Start = transform.position;
        float t = 0;

        while(t < 0.3f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(Start, landingPoint, t / 0.3f);
            yield return null;
        }
        transform.position = landingPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
                GameManager.Instance.inventoryController.AddItem(_item, gameObject);
            }
        }
    }
}
