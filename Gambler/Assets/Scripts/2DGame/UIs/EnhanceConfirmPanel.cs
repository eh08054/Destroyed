using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnhanceConfirmPanel : MonoBehaviour
{
    public TMP_Text text;
    public Button confirmButton;
    public Button cancleButton;

    private void Start()
    {
        confirmButton.onClick.AddListener(UIManager.Instance.SkillEnhancePanel.GetComponent<EnhanceController>().OnConfirm);
        cancleButton.onClick.AddListener(Cancle);
    }

    private void Cancle()
    {
        gameObject.SetActive(false);
    }
}
