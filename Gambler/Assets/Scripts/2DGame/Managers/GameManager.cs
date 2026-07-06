using Assets.PixelFantasy.PixelTileEngine.Scripts;
using CityBackgroundsCollection;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public enum GameState
{
    Playing,
    Pausing,
    Loading,
}
public class GameManager : MonoBehaviour
{
    InputManager _input = new InputManager();
    public static InputManager InputM { get { return Instance._input; } }

    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public PlayerBase PlayerBase { get; private set; }
    public GameObject InventoryPanel { get; private set; }
    public GameObject SkillPanel{ get; private set; }
    public GameObject PausePanel { get; private set; }
    public GameObject SettingsPanel { get; private set; }
    public GameObject GameEndPanel { get; private set; }
    public GameObject Background { get; private set; }
    public Transform BackgroundOnly { get; private set; }
    public Inventory PlayerInventory { get; private set; }
    public GameData GameData { get; private set; }
    public SceneChanger SceneChanger { get; private set; }
    public StageData.StageDifficulty CurrentDifficulty { get; private set; }
    public GameState gameState = GameState.Playing;

    [SerializeField] private List<StageData> stages;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject inventoryPrefab;
    [SerializeField] private GameObject skillContainerPrefab;
    [SerializeField] private GameObject pauseCanvasPrefab;
    [SerializeField] private GameObject settingsCanvasPrefab;
    [SerializeField] private GameObject GameEndCanvasPrefab;
    [SerializeField] private GameObject[] treasureBoxPrefab;
    private Platform[] platforms;

    public event Action OnClear;
    public event Action OnStageLoaded;
    public event Action OpenDialog;
    public event Action CloseDialog;
    public event Action<int> GoldChanged;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameData = new GameData();
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
            SpawnPlayer();
            CreateInventory();
            CreateSkillContainer();
            CreatePausePanel();
            CreateSettingsPanel();
            CreateGameEndPanel();
            LoadSaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadSaveData()
    {
        SaveData saveData = SaveSystem.Load();
        if (saveData != null)
        {
            foreach (var itemName in saveData.item_names)
            {
                ItemData item = Resources.Load<ItemData>("2DGame/ItemsData/" + itemName);
                InventoryPanel.GetComponentInChildren<InventoryController>().AddItem(item);
            }
            GameData.Gold = saveData.gold;
            SettingsPanel.GetComponent<SettingsController>().BGMSlider.value = saveData.BGMVolume;
            SettingsPanel.GetComponent<SettingsController>().SFXSlider.value = saveData.SFXVolume;
            SettingsPanel.GetComponent<SettingsController>().AllSlider.value = saveData.AllVolume;
            GameData.BGMVolume = saveData.BGMVolume;
            GameData.SFXVolume = saveData.SFXVolume;
            GameData.AllVolume = saveData.AllVolume;
        }
    }
    private void Start()
    {
        InputM.keyAction -= RegisterKeyActions;
        InputM.keyAction += RegisterKeyActions;
    }
    private void Update()
    {
        _input.OnUpdate();
    }
    private void SpawnPlayer()
    {
        if(Player == null)
        {
            Player = Instantiate(playerPrefab);
            DontDestroyOnLoad(Player);
            PlayerBase = Player.GetComponent<PlayerController>().player;
            PlayerBase.Init();
        }
    }
    private void CreateInventory()
    {
        if(InventoryPanel == null)
        {
            InventoryPanel = Instantiate(inventoryPrefab);
            DontDestroyOnLoad(InventoryPanel);
        }
    }
    public void CreateSkillContainer()
    {
        if(SkillPanel == null)
        {
            SkillPanel = Instantiate(skillContainerPrefab);
            DontDestroyOnLoad(SkillPanel);
        }
    }
    public void CreatePausePanel()
    {
        if(PausePanel == null)
        {
            PausePanel = Instantiate(pauseCanvasPrefab);
            DontDestroyOnLoad(PausePanel);
        }
    }
    public void CreateSettingsPanel()
    {
        if(SettingsPanel == null)
        {
            SettingsPanel = Instantiate(settingsCanvasPrefab);
            DontDestroyOnLoad(SettingsPanel);
        }
    }
    public void CreateGameEndPanel()
    {
        if(GameEndPanel == null)
        {
            GameEndPanel = Instantiate(GameEndCanvasPrefab);
            DontDestroyOnLoad(GameEndPanel);
        }
    }
    public void RegisterInventory(Inventory inventory)
    {
        PlayerInventory = inventory;
    }
    public void RegisterKeyActions()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryPanel.activeSelf)
            {
                UIManager.Instance.ClosePanel(InventoryPanel);
            }
            else
            {
                UIManager.Instance.OpenPanel(InventoryPanel);
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (SkillPanel.activeSelf)
            {
                UIManager.Instance.ClosePanel(SkillPanel);
            }
            else
            {
                UIManager.Instance.OpenPanel(SkillPanel);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.openedPanels.Count != 0)
            {
                UIManager.Instance.CloseLastPanel();
            }
            else if (!PausePanel.activeSelf)
            {
                UIManager.Instance.OpenPanel(PausePanel);
            }
        }
    }
    public void AddGold(int value)
    {
        GameData.Gold += value;
        GoldChanged?.Invoke(GameData.Gold);
    }
    public void UseGold(int value)
    {
        GameData.Gold -= value;
        GoldChanged?.Invoke(GameData.Gold);
    }
    public void ChangeHP()
    {
        Player.GetComponent<PlayerController>().InvokeHPChanged();
    }
    private void LoadStage(StageData stageData)
    {
        Player.transform.SetPositionAndRotation(stageData.PlayerSpawnPosition, Quaternion.identity);
        SetBackground(stageData);
        SpawnEnemy(stageData);
        if (stageData.totalEnemyCount > 0)
        {
            UIManager.Instance.SetMinimapPanel(stageData.enemies);
        }
        CurrentDifficulty = stageData.stageDifficulty;
    }
    public void SetBackground(StageData stageData)
    {
        Background = Instantiate(stageData.backgroundPrefab, Vector3.zero, Quaternion.identity);
        BackgroundOnly = Background.transform.GetChild(0);
        Renderer[] renderers = BackgroundOnly.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;
        foreach (var renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        BoxCollider2D col = BackgroundOnly.GetComponent<BoxCollider2D>();
        if (col == null) col = BackgroundOnly.AddComponent<BoxCollider2D>();

        col.size = bounds.size;
        col.offset = new Vector2(bounds.center.x - BackgroundOnly.transform.position.x,
                                 bounds.center.y - BackgroundOnly.transform.position.y);
        col.isTrigger = true;
        platforms = Background.GetComponentsInChildren<Platform>();
    }
    public void SpawnEnemy(StageData stageData)
    {
        int enemyIndex = 0;
        int spawnCount = 0;
        GameData.RemainedEnemyInStage = stageData.totalEnemyCount;
        foreach (var platform in platforms)
        {
            for (int i = 0; i < platform.spawnPoints.Length; i++)
            {
                var enemy = stageData.enemies[enemyIndex];
                Vector3 enemyOffset = new Vector3(0, -0.3f, 0);
                GameObject newEnemy = Instantiate(enemy.enemyData.enemyPrefab,
                   platform.spawnPoints[i].position + enemyOffset,
                   Quaternion.identity);
                EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.OnDeath += EnemyDeath;
                enemyController.OnDeath += UIManager.Instance.RefreshMinimapPanel;
                if (enemy.enemyType == EnemyData.Type.Boss)
                {
                    enemyController.OnDeath += UIManager.Instance.ShowGameClearPanel;
                }
                enemyController.InitEnemy(enemy.enemyData);
                spawnCount++;
                if (spawnCount >= enemy.count)
                {
                    enemyIndex++;
                    spawnCount = 0;
                }
                if (enemyIndex >= stageData.enemies.Count)
                {
                    return;
                }
            }
        }
    }
    public void StartDialogue()
    {
        OpenDialog?.Invoke();
        DialogSystem dialog = GameObject.FindGameObjectWithTag("DialogSystem").GetComponent<DialogSystem>();
        StartCoroutine(StartDialogueCoroutine(dialog));
    }
    public void StopDialogue()
    {
        CloseDialog?.Invoke();
    }
    private IEnumerator StartDialogueCoroutine(DialogSystem dialog)
    {
        yield return new WaitUntil(() => dialog.UpdateDialog());
    }
    public void StopTime()
    {
        Time.timeScale = 0f;
    }
    public void EnemyDeath(string enemyName)
    {
        GameData.RemainedEnemyInStage--;
        if(GameData.RemainedEnemyInStage == 0)
        {
            OnClear.Invoke();
            MakeTreasureBox();
        }
    }
    public void MakeTreasureBox()
    {
        switch (CurrentDifficulty)
        {
            case StageData.StageDifficulty.peace:
                break;
            case StageData.StageDifficulty.easy:
                Instantiate(treasureBoxPrefab[0], Player.transform.position, Quaternion.identity);
                break;
            case StageData.StageDifficulty.normal:
                Instantiate(treasureBoxPrefab[1], Player.transform.position, Quaternion.identity);
                break;
            case StageData.StageDifficulty.hard:
                Instantiate(treasureBoxPrefab[2], Player.transform.position, Quaternion.identity);
                break;
        }
    }
    public void ResetGameData()
    {
        PlayerBase.CurrentHP = PlayerBase.MaxHP;
    }
    public void ResetPanel()
    {
        InventoryPanel.SetActive(false);
        SkillPanel.SetActive(false);
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(false);
        GameEndPanel.SetActive(false);
    }
    private void OnDestroy()
    {
        if (Instance != this) { return; }
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SaveData saveData = new SaveData();
        if (PlayerInventory != null)
        {
            foreach (var itemSlot in PlayerInventory.itemSlots)
            {
                for (int i = 0; i < itemSlot.count; i++)
                {
                    saveData.item_names.Add(itemSlot.item.itemName);
                }
            }
        }
        saveData.gold = GameData.Gold;
        saveData.BGMVolume = GameData.BGMVolume;
        saveData.SFXVolume = GameData.SFXVolume;
        saveData.AllVolume = GameData.AllVolume;
        SaveSystem.Save(saveData);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MapScene")
        {
            GameData.SelectedStage = 0;
            Player.SetActive(true);
            PlayerBase.CurrentHP = PlayerBase.MaxHP;
            AudioManager.instance.PlayBGM(BGM.MAP);
        }
        else if (scene.name == "GameScene")
        {
            AudioManager.instance.PlayBGM(BGM.BATTLE);
        }
        else if (scene.name == "MenuScene")
        {
            AudioManager.instance.PlayBGM(BGM.TITLE);
            ResetGameData();
        }
        else { return; }

        if (scene.name != "MenuScene")
        {
            LoadStage(stages[GameData.SelectedStage]);
            OnStageLoaded?.Invoke();
            UIManager.Instance.HPSliderInit(PlayerBase.CurrentHP, PlayerBase.MaxHP);
            UIManager.Instance.ChangeWeaponImage(PlayerBase.ownedWeapons[PlayerBase.currentWeaponIndex % 2]);
            UIManager.Instance.SetGold(GameData.Gold);
            if (Player.GetComponent<SkillController>().EquipedSkills != null)
            {
                UIManager.Instance.SetSkillImages(Player.GetComponent<SkillController>().EquipedSkills);
            }
        }
        ResetPanel();
        SceneChanger = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
    }
}
