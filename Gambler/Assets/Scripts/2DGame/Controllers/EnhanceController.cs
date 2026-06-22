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
        EnhanceConfirmPanel.SetActive(true);
    }
    public void OnConfirm()
    {
        if(currentSlot == null) { return; }
        currentSlot.Enhance();
        foreach (var skillSlot in skillSlots) 
        {
           skillSlot.GetComponent<SkillUpgradeSlot>().Refresh();
        }
        SetDescriptionPanel(currentSlot);
        HideInhanceComment();
    }
    public void SetDescriptionText(SkillUpgradeSlot skillSlot)
    {
        text.text = string.Format(currentSlot.skillData.descriptionFormat,
               skillSlot.skillData.skillName,
               skillSlot.skillData.goldPerLevel[0],
               skillSlot.skillData.valuePerLevel[0]);
    }
    private void SetDescriptionPanel(SkillUpgradeSlot skillSlot)
    {
        SkillDescriptionPanel SDPanel = skillDescriptionPanel.GetComponent<SkillDescriptionPanel>();
        if (playerBase.ownedSkills.TryGetValue(skillSlot.skillData, out Skill skill))
        {
            SDPanel.SetPanel(skillSlot.skillData.skillName,
        string.Format(skillSlot.skillData.skillDescriptionFormat, skill.sumValue), skill.skillData.skillMaxLevel, skill.level);
        }
        else
        {
            SDPanel.SetPanel(skillSlot.skillData.skillName,
        string.Format(skillSlot.skillData.skillDescriptionFormat, 0), skillSlot.skillData.skillMaxLevel);
        }
    }
    public void HideInhanceComment()
    {
        EnhanceConfirmPanel.SetActive(false);
    }
}
