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


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public PlayerBase PlayerBase { get; private set; }
    public GameObject InventoryObject { get; private set; }
    public Inventory PlayerInventory { get; private set; }
    public GameObject Background { get; private set; }
    public Transform BackgroundOnly { get; private set; }
    public GameData GameData { get; private set; }
    InputManager _input = new InputManager();
    public static InputManager Input { get { return Instance._input; } }
    public SceneChanger SceneChanger { get; private set; }
    [SerializeField] private List<StageData> stages;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject inventoryPrefab;
    [SerializeField] private GameObject treasureBoxPrefab;
    private Platform[] platforms;

    public event Action OnClear;
    public event Action<int> OnBirth;
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
                InventoryObject.transform.GetChild(0).GetComponent<InventoryController>().AddItem(item);
            }
            GameData.gold = saveData.gold;
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
    private void CreateInventory()
    {
        if(InventoryObject == null)
        {
            InventoryObject = Instantiate(inventoryPrefab);
            DontDestroyOnLoad(InventoryObject);
        }
    }
    public void RegisterInventory(Inventory inventory)
    {
        PlayerInventory = inventory;
    }
    public void AddGold(int value)
    {
        GameData.gold += value;
        GoldChanged?.Invoke(GameData.gold);
    }
    public void UseGold(int value)
    {
        GameData.gold -= value;
        GoldChanged?.Invoke(GameData.gold);
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
            Instantiate(treasureBoxPrefab, Player.transform.position, Quaternion.identity);
        }
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
        saveData.gold = GameData.gold;
        SaveSystem.Save(saveData);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MapScene")
        {
            GameData.SelectedStage = 0;
            Player.SetActive(true);
            AudioManager.instance.PlayBGM(BGM.MAP);
        }
        if (scene.name == "MenuScene")
        {
            AudioManager.instance.PlayBGM(BGM.TITLE);
        }
        else
        {
            LoadStage(stages[GameData.SelectedStage]);
            OnStageLoaded?.Invoke();
            UIManager.Instance.HPSliderInit(PlayerBase.CurrentHP, PlayerBase.MaxHP);
            UIManager.Instance.ChangeWeaponImage(PlayerBase.ownedWeapons[PlayerBase.currentWeaponIndex % 2]);
            UIManager.Instance.SetGold(GameData.gold);
            if (Player.GetComponent<SkillController>().EquipedSkills != null)
            {
                UIManager.Instance.SetSkillImages(Player.GetComponent<SkillController>().EquipedSkills);
            }
        }
        SceneChanger = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
    }
}
