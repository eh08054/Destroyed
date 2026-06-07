using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillUpgradeSlot : MonoBehaviour
{
    public SkillData skillData;
    public TMP_Text skillLevelText;

    public void Enhance()
    {
        if(skillData.skillLevel == skillData.skillMaxLevel) { return; }
        if(GameManager.Instance.GameData.gold < skillData.goldPerLevel[skillData.skillLevel]) 
        {
            Debug.Log("골드가 부족합니다.");
            return; 
        }
        GameManager.Instance.UseGold(skillData.goldPerLevel[skillData.skillLevel]);
        GameManager.Instance.PlayerBase.ApplySkillEffect(skillData);
    }
    public void Refresh()
    {
        skillLevelText.text = $"{skillData.skillLevel} / {skillData.skillMaxLevel}";
    }
}
