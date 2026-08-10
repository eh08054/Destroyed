using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PossessSkillController : MonoBehaviour
{
    [SerializeField] private Button ActiveButton;
    [SerializeField] private Button PassiveButton;
    [SerializeField] private Button ActiveEnhanceButton;

    [SerializeField] private GameObject ActiveSkillView;
    [SerializeField] private GameObject PassiveSkillView;
    [SerializeField] private GameObject ActiveEnhanceView;

    [SerializeField] private Transform ActiveSlotParent;
    [SerializeField] private Transform PassiveSlotParent;
    [SerializeField] private Transform ActiveEnhanceParent;
    [SerializeField] private GameObject SkillPrefab;

    [SerializeField] private GameObject skillDescriptionPanel;

    [SerializeField] private TMP_Text titleText;
    private PlayerBase playerBase;

    private bool isStarted = false;

    private void Start()
    {
        ActiveButton.onClick.AddListener(OpenActiveView);
        PassiveButton.onClick.AddListener(OpenPassiveView);
        ActiveEnhanceButton.onClick.AddListener(OpenActiveEnhanceView);
        playerBase = GameManager.Instance.PlayerBase;
        isStarted = true;
        Refresh();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        skillDescriptionPanel.SetActive(false);
    }
    private void OnDisable()
    {
        RemoveAll();
        skillDescriptionPanel.SetActive(false);
    }
    public void Refresh()
    {
        var slots = gameObject.GetComponentsInChildren<SkillContainerSlot>();
        foreach(var slot in slots)
        {
            Destroy(slot.gameObject);
        }
        foreach (var (skillData, skill) in GameManager.Instance.PlayerBase.ownedSkills)
        {
            GameObject mySkill;
            if(skill is ActiveSkill activeSkill)
            {
                if(activeSkill.ActiveData.activeSkillType == ActiveSkillData.ActiveSkillType.Original)
                {
                    mySkill = Instantiate(SkillPrefab, ActiveSlotParent);
                    mySkill.GetComponentInChildren<Button>().gameObject.AddComponent<DragController>();
                }
                else
                {
                    mySkill = Instantiate(SkillPrefab, ActiveEnhanceParent);
                }
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
            enhanceText = $"\n강화 시:   <color=grey>{skill.sumValue}%</color> > <color=#62FF00>{skill.sumValue + skill.skillData.valuePerLevel[skill.level]}%</color>";
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
        ActiveEnhanceView.SetActive(false);
        ActiveSkillView.SetActive(true);
        ActiveEnhanceButton.transform.parent.gameObject.SetActive(true);
        titleText.text = "[액티브 스킬]";
    }

    private void OpenPassiveView()
    {
        ActiveSkillView.SetActive(false);
        ActiveEnhanceView.SetActive(false);
        PassiveSkillView.SetActive(true);
        ActiveEnhanceButton.transform.parent.gameObject.SetActive(false);
        titleText.text = "[패시브 스킬]";
    }

    private void OpenActiveEnhanceView()
    {
        if (ActiveSkillView.activeSelf)
        {
            ActiveSkillView.SetActive(false);
            ActiveEnhanceView.SetActive(true);
            titleText.text = "[액티브 강화]";
        }
        else
        {
            ActiveEnhanceView.SetActive(false);
            ActiveSkillView.SetActive(true);
            titleText.text = "[액티브 스킬]";
        }
    }
}
