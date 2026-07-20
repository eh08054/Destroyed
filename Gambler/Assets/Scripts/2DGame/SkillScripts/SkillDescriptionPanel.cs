using UnityEngine;
using TMPro;
public class SkillDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillLevel;
    [SerializeField] private TMP_Text skillDescription;

    public void SetPanel(string name, string levelText, string description)
    {
        skillName.text = $"[{name}]";
        skillLevel.text = levelText;
        skillDescription.text = description;
    }
}
