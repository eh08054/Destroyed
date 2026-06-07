using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class EnhanceController : MonoBehaviour
{
    [SerializeField] private List<GameObject> skillSlots;
    [SerializeField] private GameObject EnhanceConfirmPanel;
    [SerializeField] private TMP_Text text;

    private SkillUpgradeSlot currentSlot;
    private void Awake()
    {
        foreach(var x in skillSlots)
        {
            GameObject temp = x;
            SkillUpgradeSlot SUS = x.GetComponentInChildren<SkillUpgradeSlot>();
            x.GetComponentInChildren<Button>().onClick.AddListener(() => ShowInhanceComment(SUS));
            SUS.Refresh();
        }
    }
    private void ShowInhanceComment(SkillUpgradeSlot skillSlot)
    {
        currentSlot = skillSlot;
        text.text = $"{skillSlot.skillData.skillName} 강화하시겠습니까?" +
            $"\n 소모: {skillSlot.skillData.goldPerLevel[skillSlot.skillData.skillLevel]}";
        EnhanceConfirmPanel.SetActive(true);
    }
    public void OnConfirm()
    {
        Debug.Log("Hello");
        if(currentSlot == null) { return; }
        currentSlot.Enhance();
        currentSlot.Refresh();
        HideInhanceComment();
    }
    public void HideInhanceComment()
    {
        EnhanceConfirmPanel.SetActive(false);
    }
}
