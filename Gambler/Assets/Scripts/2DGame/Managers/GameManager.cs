using Assets.PixelFantasy.PixelTileEngine.Scripts;
using CityBackgroundsCollection;
using System;
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
    public GameObject Inventory { get; private set; }
    public GameObject Background { get; private set; }
    public Transform BackgroundOnly { get; private set; }
    public GameData GameData { get; private set; }
    InputManager _input = new InputManager();
    public static InputManager Input { get { return Instance._input; } }
    public SceneChanger SceneChanger { get; private set; }
    [SerializeField] private List<StageData> stages;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject inventoryPrefab;
    private Platform[] platforms;

    public event Action OnClear;
    public event Action<int> OnBirth;
    public event Action OnStageLoaded;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SpawnPlayer();
            CreateInventory();
            Debug.Log("hello");
            GameData = new GameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        SaveData saveData = SaveSystem.Load();
        if (saveData != null)
        {
            foreach (var itemName in saveData.item_names)
            {
                Item item = Resources.Load<Item>("2DGame/ItemsData/" + itemName);
                Inventory.transform.GetChild(0).GetComponent<InventoryController>().AddItem(item);
            }
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
        }
    }
    private void CreateInventory()
    {
        if(Inventory == null)
        {
            Inventory = Instantiate(inventoryPrefab);
            DontDestroyOnLoad(Inventory);
        }
    }
    private void LoadStage(StageData stageData)
    {
        Player.transform.SetPositionAndRotation(stageData.PlayerSpawnPosition, Quaternion.identity);
        SetBackground(stageData);
        SpawnEnemy(stageData);
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
                    Debug.Log(enemyIndex);
                    return;
                }
            }
        }
    }
    public void StopTime()
    {
        Time.timeScale = 0f;
    }
    public void EnemyDeath()
    {
        GameData.RemainedEnemyInStage--;
        if(GameData.RemainedEnemyInStage == 0)
        {
            OnClear.Invoke();
        }
    }
    private void OnEnable()
    {
        if (Instance != this) { return; }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        if (Instance != this) { return; }
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SaveData saveData = new SaveData();
        List<ItemSlot> MyItemSlots = Inventory.transform.GetChild(0).GetComponent<InventoryController>().Inventory.itemSlots;
        foreach(var itemSlot in MyItemSlots)
        {
            for (int i = 0; i < itemSlot.count; i++)
            {
                saveData.item_names.Add(itemSlot.item.itemName);
            }
        }
        SaveSystem.Save(saveData);
        Destroy(gameObject);
        Destroy(Player);
        Destroy(Inventory);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if(scene.name == "MapScene")
        {
            GameData.SelectedStage = 0;
        }
        if (scene.name != "MenuScene")
        {
            LoadStage(stages[GameData.SelectedStage]);
            OnStageLoaded.Invoke();
            UIManager.Instance.HPSliderInit(Player.GetComponent<PlayerController>().player.MaxHP);
        }
        SceneChanger = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
    }
}
