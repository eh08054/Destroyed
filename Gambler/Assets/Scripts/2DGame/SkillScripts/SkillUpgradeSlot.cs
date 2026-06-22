using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillUpgradeSlot : MonoBehaviour, IPointerEnterHandler
{
    public SkillData skillData;
    public TMP_Text valueSumText;
    public TMP_Text skillLevelText;
    public Image skillImage;

    public event Action<SkillUpgradeSlot> OnHoverEnter;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke(this);
    }
    public void Enhance()
    {
        GameManager.Instance.PlayerBase.ApplySkillEffect(skillData);
    }
    public void Refresh()
    {
        if (GameManager.Instance.PlayerBase.ownedSkills.TryGetValue(skillData, out Skill skill))
        {
            if (skillData is PassiveSkillData)
            {
                valueSumText.text = $"+{skill.sumValue}%";
            }
            if (skill.level == skill.skillData.skillMaxLevel)
            {
                skillLevelText.text = $"LV.MAX";
            }
            else
            {
                skillLevelText.text = $"LV.{skill.level}";
            }
        }
        else
        {
            if (skillData is PassiveSkillData)
            {
                valueSumText.text = "+0%";
            }
            else if(skillData is ActiveSkillData activeSkillData &&
                activeSkillData.requiredSkill != null)
            {
                if(GameManager.Instance.PlayerBase.ownedSkills.TryGetValue(activeSkillData.requiredSkill, out Skill requiredSkill))
                {
                    skillImage.color = new Color(1, 1, 1);
                }
                else
                {
                    skillImage.color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
            skillLevelText.text = "LV.0";
        }
    }
}
