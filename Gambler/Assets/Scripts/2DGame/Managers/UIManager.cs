using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Slider PlayerHPSlider;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private TMP_Text StageText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject skillEnhancePanel;
    [SerializeField] private GameObject MinimapPanel;
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private Image WeaponImage;
    [SerializeField] private Image[] SkillImages;
    private PlayerController playerController;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetStageText();
        playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        playerController.OnHPChanged += SetPlayerHP;
        playerController.OnDeath += ShowGameOverPanel;
        playerController.OnWeaponChanged += ChangeWeaponImage;
        GameManager.Instance.OpenDialog += ShowDialogPanel;
        GameManager.Instance.CloseDialog += HideDialogPanel;
        GameManager.Instance.GoldChanged += SetGold;
    }
    public void HPSliderInit(int current, int max)
    {
        PlayerHPSlider.maxValue = max;
        PlayerHPSlider.value = current;
        HPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;
    }
    public void SetPlayerHP(int current, int max)
    {
        PlayerHPSlider.maxValue = max;
        PlayerHPSlider.value = current;
        HPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;
    }
    public void SetGold(int goldValue)
    {
        goldText.text = "Gold: " + goldValue;
    }
    public void ShowGameClearPanel(string enemyName)
    {
        StartCoroutine(ShowGamePanelDelay());
    }
    private IEnumerator ShowGamePanelDelay()
    {
        yield return new WaitForSeconds(0.5f);
        gameClearPanel.SetActive(true);
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
    public void ShowDialogPanel()
    {
        dialogPanel.SetActive(true);
    }
    public void ShowSettingPanel()
    {
        SettingPanel.SetActive(true);
    }
    public void HideSettingPanel()
    {
        SettingPanel.SetActive(false);
    }
    public void ShowSkillEnhancePanel()
    {
        skillEnhancePanel.SetActive(true);
    }
    public void HideSkillEnhancePanel()
    {
        skillEnhancePanel.SetActive(false);
    }
    public void HideDialogPanel()
    {
        dialogPanel.SetActive(false);
    }
    public void SetMinimapPanel(List<EnemySpawnInfo> enemySpawnInfos)
    {
        MinimapPanel.GetComponent<MinimapHUD>().InitHUD(enemySpawnInfos);
    }
    public void RefreshMinimapPanel(string enemyName)
    {
        MinimapPanel.GetComponent<MinimapHUD>().RefreshHUD(enemyName);
    }
    public void ChangeWeaponImage(WeaponData weaponData)
    {
        WeaponImage.sprite = weaponData.weaponIcon;
    }

    public void RegisterSkill(int slotIndex, ActiveSkill activeSkill)
    {
        SkillImages[slotIndex].sprite = activeSkill.skillData.skillIcon;
        SkillImages[slotIndex].transform.parent.GetComponent<SkillSlotUI>().RegisterCoolDownImage(SkillImages[slotIndex]);
    }
    public void SetSkillImages(ActiveSkill[] activeSkills)
    {
        Debug.Log(activeSkills);
        for(int i = 0; i < SkillImages.Length; i++)
        {
            if (activeSkills[i] != null)
            {
                SkillImages[i].sprite = activeSkills[i].skillData.skillIcon;
                SkillImages[i].transform.parent.GetComponent<SkillSlotUI>().RegisterCoolDownImage(SkillImages[i]);
            }
        }
    }

    public void ActiveCoolDownImage(int index)
    {
        SkillImages[index].transform.parent.GetComponent<SkillSlotUI>().ActiveCoolDownImage();
    }
    public void CoolDownImage(int index, float ratio)
    {
        SkillImages[index].transform.parent.GetComponent<SkillSlotUI>().UpdateSkillSlotUI(ratio);
    }
    public void SetStageText()
    {
        if (GameData.SelectedStage == 0)
        {
            StageText.text = "SHELTER";
        }
        else
        {
            StageText.text = $"STAGE {GameData.SelectedStage}";
        }
    }
    private void OnDisable()
    {
        playerController.OnHPChanged -= SetPlayerHP;
        playerController.OnDeath -= ShowGameOverPanel;
        playerController.OnWeaponChanged -= ChangeWeaponImage;
        GameManager.Instance.OpenDialog -= ShowDialogPanel;
    }
}
