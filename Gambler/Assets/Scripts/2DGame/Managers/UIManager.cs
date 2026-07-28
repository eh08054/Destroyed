using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Menu Bar")]
    [SerializeField] private Slider PlayerHPSlider;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private TMP_Text goldText;

    [Header("Common")]
    [SerializeField] private GameObject weaponPanel;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private GameObject buffSlotPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image WeaponImage;
    [SerializeField] private Image[] SkillImages;
    [SerializeField] private float sceneFadeTime;

    [Header("Map Scene")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject skillEnhancePanel;
    [SerializeField] private GameObject activeSkillPanel;
    [SerializeField] private GameObject passiveSkillPanel;

    [Header("Game Scene")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject MinimapPanel;
    [SerializeField] private GameObject StageText;

    private PlayerController playerController;
    private Image fadeImage;
    private TMP_Text stageText;
    private CanvasGroup canvasGroup;
    public List<GameObject> openedPanels = new List<GameObject>();
    public GraphicBlink GraphicBlink { get; private set; }
    private void Awake()
    {
        Instance = this;
        canvasGroup = canvas.GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        playerController.OnHPChanged += SetPlayerHP;
        playerController.OnDeath += ShowGameOverPanel;
        playerController.OnWeaponChanged += ChangeWeaponImage;
        GameManager.Instance.OpenDialog += ShowDialogPanel;
        GameManager.Instance.CloseDialog += HideDialogPanel;
        GameManager.Instance.GoldChanged += SetGold;

        GraphicBlink = new GraphicBlink();
        fadeImage = fadePanel.GetComponent<Image>();
        StartCoroutine(FadeScene(1, 0));
        SetStageText();
    }
    public void OpenPanel(GameObject panel)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (panel == dialogPanel)
        {
            weaponPanel.SetActive(false);
        }
        panel.SetActive(true);
        openedPanels.Add(panel);
    }
    public void ClosePanel(GameObject panel, bool last = false)
    {
        GameObject entry;
        if(last == true)
        {
            entry = panel;
            openedPanels.RemoveAt(openedPanels.Count - 1);
        }
        else
        {
            entry = openedPanels.FindLast(e => e.gameObject == panel);
            if (entry == null) { return; }
            openedPanels.Remove(entry);
        }

        if (entry == dialogPanel || entry == GameManager.Instance.PausePanel)
        {
            skillPanel.SetActive(true);
            weaponPanel.SetActive(true);
        }
        panel.SetActive(false);
    }
    public void CloseLastPanel()
    {
        GameObject last = openedPanels[openedPanels.Count - 1];
        ClosePanel(last, true);
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
        goldText.text = goldValue.ToString();
    }
    public void ShowGameClearPanel(string enemyName = "", EnemyData.Type enemyType = 0)
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
        OpenPanel(dialogPanel);
    }
    public void HideDialogPanel()
    {
        ClosePanel(dialogPanel);
    }

    public void ShowSkillEnhancePanel()
    {
        OpenPanel(skillEnhancePanel);
    }
    public void HideSkillEnhancePanel()
    {
        skillEnhancePanel.SetActive(false);
    }
    public void ShowActiveSkillPanel()
    {
        passiveSkillPanel.SetActive(false);
        activeSkillPanel.SetActive(true);
    }
    public void ShowPassiveSkillPanel()
    {
        activeSkillPanel.SetActive(false);
        passiveSkillPanel.SetActive(true);
    }
    public void SetMinimapPanel(List<EnemySpawnInfo> enemySpawnInfos)
    {
        MinimapPanel.GetComponent<MinimapHUD>().InitHUD(enemySpawnInfos);
    }
    public void RefreshMinimapPanel(string enemyName, EnemyData.Type enemyType)
    {
        MinimapPanel.GetComponent<MinimapHUD>().RefreshHUD(enemyName);
    }
    public void ChangeWeaponImage(WeaponData weaponData)
    {
        WeaponImage.sprite = weaponData.weaponIcon;
    }

    public void RegisterSkill(int slotIndex, ActiveSkill activeSkill)
    {
        if (activeSkill == null)
        {
            SkillImages[slotIndex].sprite = null;
            SkillImages[slotIndex].color = new Color(0.4f, 0.4f, 0.4f);
        }
        else
        {
            SkillImages[slotIndex].sprite = activeSkill.skillData.skillIcon;
            SkillImages[slotIndex].color = new Color(1, 1, 1);
            SkillImages[slotIndex].transform.parent.GetComponent<SkillSlotUI>().RegisterCoolDownImage(SkillImages[slotIndex]);
        }
    }
    public void ResisterBuff(ActiveSkill activeSkill, Action OnEnd)
    {
        GameObject gameObject = Instantiate(buffSlotPrefab, GameManager.Instance.BuffPanel.transform);
        BuffSlot buffSlot = gameObject.GetComponent<BuffSlot>();

        buffSlot.icon.sprite = activeSkill.ActiveData.skillIcon;
        buffSlot.StartBuff(activeSkill, OnEnd);
    }
    public void SetSkillImages(ActiveSkill[] activeSkills)
    {
        for(int i = 0; i < SkillImages.Length; i++)
        {
            if (activeSkills[i] != null)
            {
                RegisterSkill(i, activeSkills[i]);
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
            //StageText.GetComponent<TMP_Text>().text = "SHELTER";
        }
        else
        {
            stageText = StageText.GetComponent<TMP_Text>();
            stageText.text = $"STAGE {GameData.SelectedStage}";
            StartCoroutine(StageBlink());
        }
    }

    public IEnumerator StageBlink()
    {
        RectTransform rect = StageText.GetComponent<RectTransform>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 StartPos = new Vector2(0, -400f);
        Vector2 endPos = new Vector2(0, -400f);

        Vector3 StartScale = Vector3.zero;
        Vector3 EndScale = Vector3.one;

        float durationTime = 2f;
        yield return StartCoroutine(GraphicBlink.MSGraphic(rect, StartPos, endPos, StartScale, EndScale, durationTime));

        StartPos = new Vector2(0, -400f);
        endPos = new Vector2(0, -rect.rect.height / 2);

        StartScale = Vector3.one;
        EndScale = new Vector3(0.5f, 0.5f, 0.5f);

        durationTime = 1f;
        yield return StartCoroutine(GraphicBlink.MSGraphic(rect, StartPos, endPos, StartScale, EndScale, durationTime));
    }
    public IEnumerator FadeScene(float start, float end, string sceneName = null)
    {
        GameManager.Instance.gameState = GameState.Loading;
        fadePanel.SetActive(true);
        yield return StartCoroutine(GraphicBlink.FadeGraphic(fadeImage, start, end, sceneFadeTime));

        if(sceneName != null)
        {
            GameManager.Instance.SceneChanger.SceneChange(sceneName);
        }
        else
        {
            fadePanel.SetActive(false);
        }
        GameManager.Instance.gameState = GameState.Playing;
    }
    public void SetHUDAlpha(float value)
    {
        canvasGroup.alpha = value;
    }
    private void OnDisable()
    {
        playerController.OnHPChanged -= SetPlayerHP;
        playerController.OnDeath -= ShowGameOverPanel;
        playerController.OnWeaponChanged -= ChangeWeaponImage;
        GameManager.Instance.OpenDialog -= ShowDialogPanel;
    }
}
