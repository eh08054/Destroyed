using CityBackgroundsCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Persistent")]
    [SerializeField] private DynamicCanvas dynamicCanvasPrefab;
    [SerializeField] private HUDPanel HUDPrefab;
    [SerializeField] private WeaponPanel weaponPrefab;
    [SerializeField] private SkillPanel skillPrefab;
    [SerializeField] private GameObject fadePrefab;
    [SerializeField] private GameObject buffPanelPrefab;
    [SerializeField] private ActiveItemPanel activeItemPrefab;
    [SerializeField] private BuffSlot buffSlotPrefab;
    [SerializeField] private BuffSlot ItemBuffSlotPrefab;
    [SerializeField] private float sceneFadeTime;

    [Header("Shelter")]
    [SerializeField] private DialogPanel dialogPrefab;
    [SerializeField] private GameObject skillEnhancePrefab;
    [SerializeField] private GameObject shopPrefab;

    [Header("Game Play")]
    [SerializeField] private GameObject gameOverPrefab;
    [SerializeField] private GameObject gameClearPrefab;
    [SerializeField] private GameObject minimapPrefab;

    [Header("Pop Up")]
    [SerializeField] private InventoryPanel inventoryPrefab;
    [SerializeField] private GameObject pausePrefab;
    [SerializeField] private GameObject settingsPrefab;
    [SerializeField] private GameObject possessSkillPrefab;
    [SerializeField] private GameObject gameEndPrefab;
    [SerializeField] private GameObject itemToolTipPrefab;

    public InventoryPanel InventoryPanel { get; private set; }
    public SkillPanel SkillPanel { get; private set; }
    public GameObject PausePanel { get; private set; }
    public GameObject SettingsPanel { get; private set; }
    public GameObject GameEndPanel { get; private set; }
    public GameObject BuffPanel { get; private set; }
    public ActiveItemPanel ActiveItemPanel { get; private set; }
    public GameObject PossessSkillPanel { get; private set; }
    public GameObject ToolTipPanel { get; private set; }
    public WeaponPanel WeaponPanel { get; private set; }
    public GameObject FadePanel { get; private set; }
    public HUDPanel HUDPanel { get; private set; }

    public DialogPanel DialogPanel { get; private set; }
    public GameObject SkillEnhancePanel { get; private set; }
    public GameObject ActiveSkillPanel { get; private set; }
    public GameObject PassiveSkillPanel { get; private set; }
    public GameObject ShopPanel { get; private set; }

    public GameObject GameOverPanel { get; private set; }
    public GameObject GameClearPanel { get; private set; }
    public GameObject MinimapPanel { get; private set; }

    public DynamicCanvas DynamicCanvas;
    public event Action CloseDialog;

    private PlayerController playerController;
    private Image fadeImage;
    private TMP_Text stageText;
    public Canvas Canvas { get; private set; }
    public List<GameObject> openedPanels = new List<GameObject>();
    public GraphicBlink GraphicBlink { get; private set; }

    private Coroutine DialogCoroutine;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        DynamicCanvas = Instantiate(dynamicCanvasPrefab);
        DontDestroyOnLoad(DynamicCanvas);

        Persistent_Panels(0);
        PopUp_Panels(0);
        Shelter_Panels(0);
        GamePlay_Panels(0);
    }
    private void Start()
    {
        playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        playerController.OnHPChanged += SetPlayerHP;
        playerController.OnDeath += ShowGameOverPanel;
        playerController.OnWeaponChanged += ChangeWeaponImage;
        GameManager.Instance.GoldChanged += SetGold;
        GameManager.InputM.keyAction -= RegisterKeyActions;
        GameManager.InputM.keyAction += RegisterKeyActions;
        SceneManager.sceneLoaded += OnSceneLoaded;

        GraphicBlink = new GraphicBlink();
        fadeImage = FadePanel.GetComponent<Image>();
        StartCoroutine(FadeScene(1, 0));
        DynamicCanvas.GamePlayGroup.SetActive(false);

        GameManager.Instance.inventoryController.SetItemToolTip(ToolTipPanel.GetComponent<ItemToolTip>());
    }
    public void RegisterKeyActions()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryPanel.gameObject.activeSelf)
            {
                ClosePanel(InventoryPanel.gameObject);
            }
            else
            {
                OpenPanel(InventoryPanel.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (PossessSkillPanel.activeSelf)
            {
                ClosePanel(PossessSkillPanel);
            }
            else
            {
                PossessSkillPanel.GetComponent<PossessSkillController>().Refresh();
                OpenPanel(PossessSkillPanel);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openedPanels.Count != 0)
            {
                CloseLastPanel();
            }
            else if (!PausePanel.activeSelf)
            {
                OpenPanel(PausePanel);
            }
        }
    }
    public void OpenPanel(GameObject panel)
    {
        EventSystem.current.SetSelectedGameObject(null);
        panel.SetActive(true);

        if(panel == ShopPanel)
        {
            WeaponPanel.gameObject.SetActive(false);
            SkillPanel.gameObject.SetActive(false);
            HUDPanel.gameObject.SetActive(false);
            DialogPanel.gameObject.SetActive(false);
        }
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

        if (entry == ShopPanel)
        {
            WeaponPanel.gameObject.SetActive(true);
            SkillPanel.gameObject.SetActive(true);
            HUDPanel.gameObject.SetActive(true);
            DialogPanel.gameObject.SetActive(true);
        }

        if (entry == DialogPanel.gameObject || entry == PausePanel)
        {
            SkillPanel.gameObject.SetActive(true);
            WeaponPanel.gameObject.SetActive(true);
        }
        if(entry == DialogPanel.gameObject)
        {
            if(DialogCoroutine != null) { StopCoroutine(DialogCoroutine); }
            panel.SetActive(false);
            CloseDialog?.Invoke();
            return;
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
        HUDPanel.PlayerHPSlider.maxValue = max;
        HUDPanel.PlayerHPSlider.value = current;
        HUDPanel.HPText.text = HUDPanel.PlayerHPSlider.value + "/" + HUDPanel.PlayerHPSlider.maxValue;
    }
    public void SetPlayerHP(int current, int max)
    {
        HUDPanel.PlayerHPSlider.maxValue = max;
        HUDPanel.PlayerHPSlider.value = current;
        HUDPanel.HPText.text = HUDPanel.PlayerHPSlider.value + "/" + HUDPanel.PlayerHPSlider.maxValue;
    }
    public void SetGold(int goldValue)
    {
        HUDPanel.GoldText.text = goldValue.ToString();
    }
    public void ShowGameClearPanel(string enemyName = "", EnemyData.Type enemyType = 0)
    {
        StartCoroutine(ShowGamePanelDelay());
    }
    private IEnumerator ShowGamePanelDelay()
    {
        yield return new WaitForSeconds(0.5f);
        GameClearPanel.SetActive(true);
    }
    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
    public void StartDialogue()
    {
        ShowDialogPanel();
        DialogSystem dialog = GameObject.FindGameObjectWithTag("DialogSystem").GetComponent<DialogSystem>();
        DialogCoroutine = StartCoroutine(DialogueCoroutine(dialog));
    }
    private IEnumerator DialogueCoroutine(DialogSystem dialog)
    {
        yield return new WaitUntil(() => dialog.UpdateDialog());
    }
    public void ShowDialogPanel()
    {
        OpenPanel(DialogPanel.gameObject);
    }
    public void HideDialogPanel()
    {
        ClosePanel(DialogPanel.gameObject);
    }

    public void ShowSkillEnhancePanel()
    {
        OpenPanel(SkillEnhancePanel);
    }
    public void HideSkillEnhancePanel()
    {
        SkillEnhancePanel.SetActive(false);
    }
    public void ShowActiveSkillPanel()
    {
        PassiveSkillPanel.SetActive(false);
        ActiveSkillPanel.SetActive(true);
    }
    public void ShowPassiveSkillPanel()
    {
        ActiveSkillPanel.SetActive(false);
        PassiveSkillPanel.SetActive(true);
    }
    public void SetMinimapPanel(List<EnemySpawnInfo> enemySpawnInfos)
    {
        MinimapPanel.GetComponentInChildren<MinimapHUD>().InitHUD(enemySpawnInfos);
    }
    public void RefreshMinimapPanel(string enemyName, EnemyData.Type enemyType)
    {
        MinimapPanel.GetComponentInChildren<MinimapHUD>().RefreshHUD(enemyName);
    }
    public void ChangeWeaponImage(WeaponData weaponData)
    {
        WeaponPanel.WeaponImage.sprite = weaponData.weaponIcon;
    }

    public void RegisterSkill(int slotIndex, ActiveSkill activeSkill)
    {
        if (activeSkill == null)
        {
            SkillPanel.SkillImages[slotIndex].sprite = null;
            SkillPanel.SkillImages[slotIndex].color = new Color(0.4f, 0.4f, 0.4f);
        }
        else
        {
            SkillPanel.SkillImages[slotIndex].sprite = activeSkill.skillData.skillIcon;
            SkillPanel.SkillImages[slotIndex].color = new Color(1, 1, 1);
            SkillPanel.SkillImages[slotIndex].transform.parent.GetComponent<SkillSlotUI>().RegisterCoolDownImage(SkillPanel.SkillImages[slotIndex]);
        }
    }
    public void ResisterBuff(ActiveSkill activeSkill, Action OnEnd)
    {
        BuffSlot buffSlot = Instantiate(buffSlotPrefab, BuffPanel.transform);

        buffSlot.icon.sprite = activeSkill.ActiveData.skillIcon;
        buffSlot.StartBuff(activeSkill, OnEnd);
    }
    public void ResisterItem(ItemData item, Action OnEnd)
    {
        BuffSlot itemBuffSlot = Instantiate(ItemBuffSlotPrefab, ActiveItemPanel.transform);
        itemBuffSlot.icon.sprite = item.ItemIcon;
        itemBuffSlot.StartBuff(item, OnEnd);
        ActiveItemPanel.ActiveBuffSlots[item] = itemBuffSlot;
    }
    public void RemoveItem(ItemData item)
    {
        if(ActiveItemPanel.ActiveBuffSlots.TryGetValue(item, out var slot))
        {
            ActiveItemPanel.ActiveBuffSlots.Remove(item);
            Destroy(slot.gameObject);
        }
    }
    public void SetSkillImages(ActiveSkill[] activeSkills)
    {
        for(int i = 0; i < SkillPanel.SkillImages.Length; i++)
        {
            if (activeSkills[i] != null)
            {
                RegisterSkill(i, activeSkills[i]);
            }
        }
    }

    public void ActiveCoolDownImage(int index)
    {
        SkillPanel.SkillImages[index].transform.parent.GetComponent<SkillSlotUI>().ActiveCoolDownImage();
    }
    public void CoolDownImage(int index, float ratio)
    {
        SkillPanel.SkillImages[index].transform.parent.GetComponent<SkillSlotUI>().UpdateSkillSlotUI(ratio);
    }
    public void SetStageText()
    {
        if (GameData.SelectedStage == 0)
        {
            //StageText.GetComponent<TMP_Text>().text = "SHELTER";
        }
        else
        {
            stageText = GameObject.Find("StageText").GetComponent<TMP_Text>();
            stageText.text = $"STAGE {GameData.SelectedStage}";
            StartCoroutine(StageBlink());
        }
    }

    public IEnumerator StageBlink()
    {
        RectTransform rect = stageText.gameObject.GetComponent<RectTransform>();
        RectTransform canvasRect = Canvas.GetComponent<RectTransform>();

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
        yield return StartCoroutine(GraphicBlink.FadeGraphic(fadeImage, start, end, sceneFadeTime));

        if (sceneName != null)
        {
            GameManager.Instance.SceneChanger.SceneChange(sceneName);
        }
        GameManager.Instance.gameState = GameState.Playing;
    }
    public void SetHUDAlpha(float value)
    {
        
    }
    public void ResetPanel()
    {
        PopUp_Panels(1);
    }
    private void OnDestroy()
    {
        //playerController.OnHPChanged -= SetPlayerHP;
        //playerController.OnDeath -= ShowGameOverPanel;
        //playerController.OnWeaponChanged -= ChangeWeaponImage;
        //GameManager.Instance.OpenDialog -= ShowDialogPanel;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetPanel();
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (scene.name == "MapScene")
        {
            DynamicCanvas.ShelterGroup.SetActive(true);
            DynamicCanvas.GamePlayGroup.SetActive(false);
        }
        else if (scene.name == "GameScene")
        {
            DynamicCanvas.ShelterGroup.SetActive(false);
            DynamicCanvas.GamePlayGroup.SetActive(true);
            StartCoroutine(FadeScene(1, 0));
            SetStageText();
        }
        else if (scene.name == "MenuScene")
        {
            DynamicCanvas.ShelterGroup.SetActive(false);
            DynamicCanvas.GamePlayGroup.SetActive(false);
        }
        else { return; }
    }
    public void Shelter_Panels(int order)
    {
        switch (order)
        {
            case 0:
                DialogPanel = Instantiate(dialogPrefab, DynamicCanvas.ShelterGroup.transform);
                SkillEnhancePanel = Instantiate(skillEnhancePrefab, DynamicCanvas.ShelterGroup.transform);
                ShopPanel = Instantiate(shopPrefab, DynamicCanvas.ShelterGroup.transform);
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }
    public void GamePlay_Panels(int order)
    {
        switch (order)
        {
            case 0:
                GameOverPanel = Instantiate(gameOverPrefab, DynamicCanvas.GamePlayGroup.transform);
                GameClearPanel = Instantiate(gameClearPrefab, DynamicCanvas.GamePlayGroup.transform);
                MinimapPanel = Instantiate(minimapPrefab, DynamicCanvas.GamePlayGroup.transform);
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }
    public void Persistent_Panels(int order)
    {
        switch (order)
        {
            case 0:
                BuffPanel = Instantiate(buffPanelPrefab, DynamicCanvas.PersistentGroup.transform);
                ActiveItemPanel = Instantiate(activeItemPrefab, DynamicCanvas.PersistentGroup.transform);
                SkillPanel = Instantiate(skillPrefab, DynamicCanvas.PersistentGroup.transform);
                HUDPanel = Instantiate(HUDPrefab, DynamicCanvas.PersistentGroup.transform);
                WeaponPanel = Instantiate(weaponPrefab, DynamicCanvas.PersistentGroup.transform);
                FadePanel = Instantiate(fadePrefab, DynamicCanvas.PersistentGroup.transform);
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }
    public void PopUp_Panels(int order)
    {
        switch (order)
        {
            case 0:
                PausePanel = Instantiate(pausePrefab, DynamicCanvas.PopUpGroup.transform);
                SettingsPanel = Instantiate(settingsPrefab, DynamicCanvas.PopUpGroup.transform);
                GameEndPanel = Instantiate(gameEndPrefab, DynamicCanvas.PopUpGroup.transform);
                PossessSkillPanel = Instantiate(possessSkillPrefab, DynamicCanvas.PopUpGroup.transform);
                InventoryPanel = Instantiate(inventoryPrefab, DynamicCanvas.PopUpGroup.transform);
                ToolTipPanel = Instantiate(itemToolTipPrefab, DynamicCanvas.PopUpGroup.transform);
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }
}
