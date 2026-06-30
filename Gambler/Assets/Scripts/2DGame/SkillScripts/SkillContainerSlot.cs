using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillContainerSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillLevel;

    public Skill skill;

    public event Action<SkillContainerSlot> OnHoverEnter;
    public event Action OnHoverExit;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter?.Invoke(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit?.Invoke();
    }
    public void RegisterSkillSlot(Skill skill)
    {
        this.skill = skill;
        skillIcon.sprite = skill.skillData.skillIcon;
        skillName.text = skill.skillData.skillName;
        if (skill.level < skill.skillData.skillMaxLevel)
        {
            skillLevel.text = $"LV.{skill.level}";
        }
        else
        {
            skillLevel.text = "LV.MAX";
        }
    }
}
