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
            SkillUpgradeSlot SUS = x.GetComponentInChildren<SkillUpgradeSlot>();
            x.GetComponentInChildren<Button>().onClick.AddListener(() => ShowInhanceComment(SUS));
            SUS.OnHoverEnter += ActivateSDPanel;
            SUS.OnHoverExit += DeActivateSDPanel;
            SUS.Refresh();
        }
        currentSlot = skillSlots[0].GetComponent<SkillUpgradeSlot>();
        playerBase = GameManager.Instance.PlayerBase;
    }
    private void OnDisable()
    {
        skillDescriptionPanel.SetActive(false);
    }
    private void OnDestroy()
    {
        foreach (var x in skillSlots)
        {
            SkillUpgradeSlot SUS = x.GetComponentInChildren<SkillUpgradeSlot>();
            x.GetComponentInChildren<Button>().onClick.RemoveListener(() => ShowInhanceComment(SUS));
        }
    }
    private void ShowInhanceComment(SkillUpgradeSlot skillSlot)
    {
        currentSlot = skillSlot;
        if (playerBase.ownedSkills.TryGetValue(skillSlot.skillData, out Skill skill))
        {
            if(skill.level == skill.skillData.skillMaxLevel) { return; }
            text.text = string.Format(currentSlot.skillData.descriptionFormat,
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
                if (activeSkillData.attackSkillType == ActiveSkillData.AttackSkillType.Original)
                {
                    text.text = currentSlot.skillData.descriptionFormat;
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
        UIManager.Instance.OpenPanel(EnhanceConfirmPanel);
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
        HideInhanceComment();
    }
    public void SetDescriptionText(SkillUpgradeSlot skillSlot)
    {
        text.text = string.Format(currentSlot.skillData.descriptionFormat,
               skillSlot.skillData.skillName,
               skillSlot.skillData.goldPerLevel[0],
               skillSlot.skillData.valuePerLevel[0]);
    }
    private void ActivateSDPanel(SkillUpgradeSlot skillSlot)
    {
        if (!skillDescriptionPanel.activeSelf)
        {
            skillDescriptionPanel.SetActive(true);
        }
        SkillDescriptionPanel SDPanel = skillDescriptionPanel.GetComponent<SkillDescriptionPanel>();

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
            if(skillSlot.skillData is ActiveSkillData activeSkillData && activeSkillData.attackSkillType == ActiveSkillData.AttackSkillType.Original)
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
        if (skillDescriptionPanel.activeSelf)
        {
            skillDescriptionPanel.SetActive(false);
        }
    }
    public void HideInhanceComment()
    {
        UIManager.Instance.ClosePanel(EnhanceConfirmPanel);
    }
}
