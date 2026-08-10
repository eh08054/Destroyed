using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class EnhanceController : MonoBehaviour
{
    [SerializeField] private List<GameObject> skillSlots;
    [SerializeField] private EnhanceConfirmPanel EnhanceConfirmPrefab;

    public EnhanceConfirmPanel EnhanceConfirmPanel { get; private set; }
    public GameObject SkillDescriptionPanel;
    public GameObject ActiveSkillPanel;
    public GameObject PassiveSkillPanel;

    private SkillUpgradeSlot currentSlot;
    private PlayerBase playerBase;
    private void Start()
    {
        foreach(var x in skillSlots)
        {
            SkillUpgradeSlot SUS = x.GetComponentInChildren<SkillUpgradeSlot>();
            x.GetComponentInChildren<Button>().onClick.AddListener(() => ShowEnhanceComment(SUS));
            SUS.OnHoverEnter += ActivateSDPanel;
            SUS.OnHoverExit += DeActivateSDPanel;
            SUS.Refresh();
        }

        currentSlot = skillSlots[0].GetComponent<SkillUpgradeSlot>();
        playerBase = GameManager.Instance.PlayerBase;
        EnhanceConfirmPanel = Instantiate(EnhanceConfirmPrefab, UIManager.Instance.DynamicCanvas.PopUpGroup.transform);
        EnhanceConfirmPanel.gameObject.SetActive(false);
        SkillDescriptionPanel.SetActive(false);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        DeActivateSDPanel();
    }
    private void OnDestroy()
    {
        foreach (var x in skillSlots)
        {
            SkillUpgradeSlot SUS = x.GetComponentInChildren<SkillUpgradeSlot>();
            x.GetComponentInChildren<Button>().onClick.RemoveListener(() => ShowEnhanceComment(SUS));
        }
    }
    private void ShowEnhanceComment(SkillUpgradeSlot skillSlot)
    {
        currentSlot = skillSlot;
        if (playerBase.ownedSkills.TryGetValue(skillSlot.skillData, out Skill skill))
        {
            if(skill.level == skill.skillData.skillMaxLevel) { return; }
            EnhanceConfirmPanel.text.text = string.Format(currentSlot.skillData.descriptionFormat,
            skillSlot.skillData.skillName,
            skillSlot.skillData.goldPerLevel[skill.level],
            skillSlot.skillData.valuePerLevel[skill.level]);
        }
        else
        {
            if (skillSlot.skillData is ActiveSkillData activeSkillData)
            {
                if (activeSkillData.requiredSkill != null 
                    && !GameManager.Instance.PlayerBase.ownedSkills.TryGetValue(activeSkillData.requiredSkill, out _)){ return; }
                if (activeSkillData.activeSkillType == ActiveSkillData.ActiveSkillType.Original)
                {
                    EnhanceConfirmPanel.text.text = string.Format(currentSlot.skillData.descriptionFormat,
                        skillSlot.skillData.skillName,
                        skillSlot.skillData.goldPerLevel[0]);
                }
                else
                {
                    SetDescriptionText(skillSlot);
                }
            }
            else
            {
                SetDescriptionText(skillSlot);
            }
        }
        UIManager.Instance.OpenPanel(EnhanceConfirmPanel.gameObject);
    }
    public void OnConfirm()
    {
        if(currentSlot == null) { return; }
        currentSlot.Enhance();
        foreach (var skillSlot in skillSlots) 
        {
           skillSlot.GetComponent<SkillUpgradeSlot>().Refresh();
        }
        ActivateSDPanel(currentSlot);
        HideEnhanceComment();
    }
    public void SetDescriptionText(SkillUpgradeSlot skillSlot)
    {
        EnhanceConfirmPanel.text.text = string.Format(currentSlot.skillData.descriptionFormat,
               skillSlot.skillData.skillName,
               skillSlot.skillData.goldPerLevel[0],
               skillSlot.skillData.valuePerLevel[0]);
    }
    private void ActivateSDPanel(SkillUpgradeSlot skillSlot)
    {
        if (!SkillDescriptionPanel.activeSelf)
        {
            SkillDescriptionPanel.SetActive(true);
        }
        SkillDescriptionPanel SDPanel = SkillDescriptionPanel.GetComponent<SkillDescriptionPanel>();

        string levelText;
        float sumValue;
        string enhanceText;
        if (playerBase.ownedSkills.TryGetValue(skillSlot.skillData, out Skill skill))
        {
            if(skill.level == skill.skillData.skillMaxLevel)
            {
                levelText = "LV.MAX";
                enhanceText = "";
            }
            else
            {
                levelText = $"LV.{skill.level} / {skill.skillData.skillMaxLevel}";
                enhanceText = $"\n강화 시:   <color=grey>{skill.sumValue}%</color> > <color=#62FF00>{skill.sumValue + skill.skillData.valuePerLevel[skill.level]}%</color>";
            }
            sumValue = skill.sumValue;
        }
        else
        {
            if(skillSlot.skillData is ActiveSkillData activeSkillData && activeSkillData.activeSkillType == ActiveSkillData.ActiveSkillType.Original)
            {
                levelText = "미해금";
                enhanceText = "";               
            }
            else
            {
                levelText = $"LV.0 / {skillSlot.skillData.skillMaxLevel}";
                enhanceText = $"\n강화 시:   <color=grey>0%</color> > <color=#62FF00>{skillSlot.skillData.valuePerLevel[0]}%</color>";
            }
            sumValue = 0;
        }
        SDPanel.SetPanel(skillSlot.skillData.skillName, levelText, string.Format(skillSlot.skillData.skillDescriptionFormat, sumValue) + enhanceText);
    }
    private void DeActivateSDPanel()
    {
        if (SkillDescriptionPanel.activeSelf)
        {
            SkillDescriptionPanel.SetActive(false);
        }
    }
    public void OpenPassivePanel()
    {
        ActiveSkillPanel.SetActive(false);
        PassiveSkillPanel.SetActive(true);
    }
    public void OpenActivePanel()
    {
        ActiveSkillPanel.SetActive(true);
        PassiveSkillPanel.SetActive(false);
    }
    public void HideEnhanceComment()
    {
        UIManager.Instance.ClosePanel(EnhanceConfirmPanel.gameObject);
    }
}
