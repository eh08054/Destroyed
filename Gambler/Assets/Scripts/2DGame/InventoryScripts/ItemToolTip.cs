using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemDescription;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void SetPanel(string name, Sprite icon, string description)
    {
        itemName.text = name;
        itemImage.sprite = icon;
        itemDescription.text = description;
    }
}
