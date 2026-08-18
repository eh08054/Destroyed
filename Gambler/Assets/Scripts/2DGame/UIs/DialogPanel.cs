using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogPanel : MonoBehaviour
{
    public Image ImageArrow; 
    public TMP_Text DialogName;
    public TMP_Text Dialog;
    public Image SpriteRenderer;

    public Button Upgradebutton;
    public Button ShopButton;

    private void Start()
    {
        Upgradebutton.onClick.AddListener(() => UIManager.Instance.OpenPanel(UIManager.Instance.SkillEnhancePanel));
        ShopButton.onClick.AddListener(() => UIManager.Instance.OpenPanel(UIManager.Instance.ShopPanel));
        gameObject.SetActive(false);
    }
}
