using UnityEngine;
using TMPro;
public class SkillDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillDescription;
    [SerializeField] private TMP_Text skillLevel;

    public void SetPanel(string name, string description, int maxLevel, int level = 0)
    {
        skillName.text = name;
        skillDescription.text = description;
        if (maxLevel == level)
        {
            skillLevel.text = "LV.MAX";
        }
        else
        {
            skillLevel.text = $"LV.{level} / {maxLevel}";
        }
    }
}
