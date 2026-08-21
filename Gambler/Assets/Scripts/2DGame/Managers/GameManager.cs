
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject Background { get; private set; }
    public Transform BackgroundOnly { get; private set; }
    public Inventory PlayerInventory { get; private set; }
    public GameData GameData { get; private set; }
    public SceneChanger SceneChanger { get; private set; }
    public StageData.StageDifficulty CurrentDifficulty { get; private set; }
    public DialogSystem DialogSystem;
    public InventoryController inventoryController;
    public GameState gameState = GameState.Playing;

    [SerializeField] private List<StageData> stages;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] treasureBoxPrefab;
    private Platform[] platforms;

    public event Action OnClear;
    public event Action OnStageLoaded;
    public event Action<int> GoldChanged;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameData = new GameData();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Instantiate(DialogSystem);
            inventoryController = gameObject.AddComponent<InventoryController>();
            DontDestroyOnLoad(gameObject);
            SpawnPlayer();
            LoadSaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        inventoryController.InitializeInventory(PlayerInventory);
        UIManager.Instance.InventoryPanel.InitializeUI(inventoryController.Inventory);
        SettingsController settings = UIManager.Instance.SettingsPanel.GetComponent<SettingsController>();
        settings.BGMSlider.value = GameData.BGMVolume;
        settings.SFXSlider.value = GameData.SFXVolume;
        settings.AllSlider.value = GameData.AllVolume;
        settings.HUDSlider.value = GameData.HUDAlpha;
        GameData.BGMVolume = GameData.BGMVolume;
        GameData.SFXVolume = GameData.SFXVolume;
        GameData.AllVolume = GameData.AllVolume;
        GameData.HUDAlpha = GameData.HUDAlpha;
    }
    public void LoadSaveData()
    {
        SaveData saveData = SaveSystem.Load();
        if (saveData != null)
        {
            PlayerInventory = new Inventory
            {
                itemSlots = new List<ItemSlot>(),
                maxSlotCount = 30
            };
            foreach (var saveName in saveData.item_names)
            {
                ItemData item = Resources.Load<ItemData>("2DGame/ItemsData/" + saveName);
                PlayerInventory.AddItem(item);
            }

            GameData.Gold = saveData.gold;
            GameData.BGMVolume = saveData.BGMVolume;
            GameData.SFXVolume = saveData.SFXVolume;
            GameData.AllVolume = saveData.AllVolume;
            GameData.HUDAlpha = saveData.HUDAlpha;
        }
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
    public void RegisterInventory(Inventory inventory)
    {
        PlayerInventory = inventory;
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
                Vector3 enemyOffset = enemy.enemyData.offset;
                GameObject newEnemy = Instantiate(enemy.enemyData.enemyPrefab,
                   platform.spawnPoints[i].position + enemyOffset,
                   Quaternion.identity);
                EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.OnDeath += EnemyDeath;
                enemyController.OnDeath += UIManager.Instance.RefreshMinimapPanel;
                enemyController.InitEnemy(enemy.enemyData, enemy.enemyType);
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
    public void StopTime()
    {
        Time.timeScale = 0f;
    }
    public void EnemyDeath(string enemyName, EnemyData.Type enemyType)
    {
        GameData.RemainedEnemyInStage--;
        if(GameData.RemainedEnemyInStage == 0)
        {
            OnClear?.Invoke();
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
    private void OnApplicationQuit()
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
                    saveData.item_names.Add(itemSlot.item.saveName);
                }
            }
        }
        saveData.gold = GameData.Gold;
        saveData.BGMVolume = GameData.BGMVolume;
        saveData.SFXVolume = GameData.SFXVolume;
        saveData.AllVolume = GameData.AllVolume;
        saveData.HUDAlpha = GameData.HUDAlpha;
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
            UIManager.Instance.SetHUDAlpha(GameData.HUDAlpha);
        }
        else if (scene.name == "GameScene")
        {
            AudioManager.instance.PlayBGM(BGM.BATTLE);
            UIManager.Instance.SetHUDAlpha(GameData.HUDAlpha);
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
            UIManager.Instance.SetGold(GameData.Gold);
            if (Player.GetComponent<SkillController>().EquipedSkills != null)
            {
                UIManager.Instance.SetSkillImages(Player.GetComponent<SkillController>().EquipedSkills);
            }
        }
        SceneChanger = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
    }
}
