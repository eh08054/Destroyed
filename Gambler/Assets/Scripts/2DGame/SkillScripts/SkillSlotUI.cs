using UnityEngine;
using UnityEngine.UI;
public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image coolDownImage;

    public void RegisterCoolDownImage(Image image)
    {
        coolDownImage.sprite = image.sprite;
    }
    public void ActiveCoolDownImage()
    {
        coolDownImage.gameObject.SetActive(true);
    }
    public void UpdateSkillSlotUI(float ratio)
    {
        coolDownImage.fillAmount = ratio;
        coolDownImage.gameObject.SetActive(ratio > 0f);
    }
}
