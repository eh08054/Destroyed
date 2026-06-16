using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillUpgradeSlot : MonoBehaviour, IPointerEnterHandler
{
    public SkillData skillData;
    public TMP_Text skillLevelText;

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
            skillLevelText.text = $"{skill.level} / {skillData.skillMaxLevel}";
        }
        else
        {
            skillLevelText.text = $"0 / {skillData.skillMaxLevel}";
        }
    }
}
