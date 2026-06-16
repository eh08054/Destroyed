using UnityEngine;
using TMPro;
public class SkillDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillDescription;

    public void SetPanel(string name, string description)
    {
        skillName.text = name;
        skillDescription.text = description;
    }
}
