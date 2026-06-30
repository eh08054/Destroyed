using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PossessSkillController : MonoBehaviour
{
    [SerializeField] private Button ActiveButton;
    [SerializeField] private Button PassiveButton;

    [SerializeField] private GameObject ActiveSkillView;
    [SerializeField] private GameObject PassiveSkillView;

    [SerializeField] private Transform ActiveSlotParent;
    [SerializeField] private Transform PassiveSlotParent;
    [SerializeField] private GameObject SkillPrefab;

    [SerializeField] private GameObject skillDescriptionPanel;

    private PlayerBase playerBase;

    private void Start()
    {
        ActiveButton.onClick.AddListener(OpenActiveView);
        PassiveButton.onClick.AddListener(OpenPassiveView);
        playerBase = GameManager.Instance.PlayerBase;
    }

    private void OnEnable()
    {
        Refresh();
        skillDescriptionPanel.SetActive(false);
    }
    private void OnDisable()
    {
        RemoveAll();
        skillDescriptionPanel.SetActive(false);
    }
    private void Refresh()
    {
        foreach (var (skillData, skill) in GameManager.Instance.PlayerBase.ownedSkills)
        {
            GameObject mySkill;
            if(skill is ActiveSkill)
            {
                mySkill = Instantiate(SkillPrefab, ActiveSlotParent);
            }
            else if(skill is PassiveSkill)
            {
                mySkill = Instantiate(SkillPrefab, PassiveSlotParent);
            }
            else { return; }
            var SCS = mySkill.GetComponent<SkillContainerSlot>();
            SCS.RegisterSkillSlot(skill);
            SCS.OnHoverEnter += ActivateSDPanel;
            SCS.OnHoverExit += DeActivateSDPanel;
        }
    }
    private void RemoveAll()
    {
        foreach(Transform child in ActiveSlotParent)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in PassiveSlotParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ActivateSDPanel(SkillContainerSlot skillSlot)
    {
        if (!skillDescriptionPanel.activeSelf)
        {
            skillDescriptionPanel.SetActive(true);
        }
        SkillDescriptionPanel SDPanel = skillDescriptionPanel.GetComponent<SkillDescriptionPanel>();

        string levelText;
        float sumValue;
        string enhanceText;
        Skill skill = skillSlot.skill;
        if (skill.level == skill.skillData.skillMaxLevel)
        {
            levelText = "LV.MAX";
            enhanceText = "";
        }
        else
        {
            levelText = $"LV.{skill.level} / {skill.skillData.skillMaxLevel}";
            enhanceText = $"\n°­È­ ½Ã:   <color=grey>{skill.sumValue}%</color> > <color=#62FF00>{skill.sumValue + skill.skillData.valuePerLevel[skill.level]}%</color>";
        }
        sumValue = skill.sumValue;
        SDPanel.SetPanel(skill.skillData.skillName, levelText, string.Format(skill.skillData.skillDescriptionFormat, sumValue) + enhanceText);
    }
    private void DeActivateSDPanel()
    {
        if (skillDescriptionPanel.activeSelf)
        {
            skillDescriptionPanel.SetActive(false);
        }
    }

    private void OpenActiveView()
    {
        PassiveSkillView.SetActive(false);
        ActiveSkillView.SetActive(true);
    }

    private void OpenPassiveView()
    {
        ActiveSkillView.SetActive(false);
        PassiveSkillView.SetActive(true);
    }
}
