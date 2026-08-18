using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopDescriptionPanel : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text itemPrice;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void SetDescriptionPanel(Sprite sprite, string name, string description, int price)
    {
        itemImage.sprite = sprite;
        itemName.text = name;
        itemDescription.text = description;
        itemPrice.text = price.ToString();
    }
}
