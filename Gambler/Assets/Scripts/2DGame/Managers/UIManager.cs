using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    [SerializeField] private Image WeaponImage;
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
    public void HPSliderInit(int current)
    {
        PlayerHPSlider.maxValue = GameManager.Instance.PlayerBase.MaxHP;
        PlayerHPSlider.value = current;
        HPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;
    }
    public void SetPlayerHP(int current)
    {
        PlayerHPSlider.value = current;
        HPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;
    }
    public void SetGold(int goldValue)
    {
        goldText.text = "Gold: " + goldValue;
    }
    public void ShowGameClearPanel()
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
    public void ChangeWeaponImage(WeaponData weaponData)
    {
        WeaponImage.sprite = weaponData.weaponIcon;
    }
    public void SetStageText()
    {
        if (GameData.SelectedStage == 0)
        {
            StageText.text = "SHELTER";
        }
        else if(GameData.SelectedStage == 1)
        {
            StageText.text = "STAGE 1";
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
