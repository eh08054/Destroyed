using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class EnhanceController : MonoBehaviour
{
    [SerializeField] private List<GameObject> skillSlots;
    [SerializeField] private GameObject EnhanceConfirmPanel;
    [SerializeField] private GameObject skillDescriptionPanel;
    [SerializeField] private TMP_Text text;

    private SkillUpgradeSlot currentSlot;
    private PlayerBase playerBase;
    private void Awake()
    {
        foreach(var x in skillSlots)
        {
            GameObject temp = x;
            SkillUpgradeSlot SUS = x.GetComponentInChildren<SkillUpgradeSlot>();
            x.GetComponentInChildren<Button>().onClick.AddListener(() => ShowInhanceComment(SUS));
            SUS.OnHoverEnter += SetDescriptionPanel;
            SUS.Refresh();
        }
        currentSlot = skillSlots[0].GetComponent<SkillUpgradeSlot>();
        playerBase = GameManager.Instance.PlayerBase;
    }
    private void OnEnable()
    {
        skillDescriptionPanel.SetActive(true);
    }
    private void OnDisable()
    {
        skillDescriptionPanel.SetActive(false);
    }
    private void ShowInhanceComment(SkillUpgradeSlot skillSlot)
    {
        currentSlot = skillSlot;
        if (playerBase.ownedSkills.TryGetValue(skillSlot.skillData, out Skill skill))
        {
            text.text = string.Format(currentSlot.skillData.descriptionFormat,
            skillSlot.skillData.skillName,
            skillSlot.skillData.goldPerLevel[skill.level],
            skillSlot.skillData.valuePerLevel[skill.level]);
        }
        else
        {
            text.text = string.Format(currentSlot.skillData.descriptionFormat,
            skillSlot.skillData.skillName,
            skillSlot.skillData.goldPerLevel[0],
            skillSlot.skillData.valuePerLevel[0]);
        }
        EnhanceConfirmPanel.SetActive(true);
    }
    public void OnConfirm()
    {
        if(currentSlot == null) { return; }
        currentSlot.Enhance();
        currentSlot.Refresh();
        SetDescriptionPanel(currentSlot);
        HideInhanceComment();
    }
    private void SetDescriptionPanel(SkillUpgradeSlot skillSlot)
    {
        if (playerBase.ownedSkills.TryGetValue(skillSlot.skillData, out Skill skill))
        {
            skillDescriptionPanel.GetComponent<SkillDescriptionPanel>().SetPanel(skillSlot.skillData.skillName,
        string.Format(skillSlot.skillData.skillDescriptionFormat, skill.sumValue));
        }
        else
        {
            skillDescriptionPanel.GetComponent<SkillDescriptionPanel>().SetPanel(skillSlot.skillData.skillName,
        string.Format(skillSlot.skillData.skillDescriptionFormat, 0));
        }
    }
    public void HideInhanceComment()
    {
        EnhanceConfirmPanel.SetActive(false);
    }
}
