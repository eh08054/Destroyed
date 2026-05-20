using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public GameObject BackGround { get; private set; }
    public GameData GameData { get; private set; }
    [field: SerializeField] public SceneChanger SceneChanger { get; private set; }
    [SerializeField] private GameObject Ground;
    [SerializeField] private List<StageData> stages;
    [SerializeField] private GameObject playerPrefab;
    private int PPU = 32;

    public event Action OnClear;
    private void Awake()
    {
        Instance = this;
        GameData = new GameData();
        LoadStage(stages[GameData.SelectedStage]);
    }
    private void Start()
    {
        PlayerController playerController = Player.GetComponent<PlayerController>();
    }
    private void LoadStage(StageData stageData)
    {
        Player = Instantiate(playerPrefab, stageData.PlayerSpawnPosition, Quaternion.identity);
        BackGround = Instantiate(stageData.backGroundPrefab, Vector3.zero, Quaternion.identity);
        BackGround.AddComponent<BoxCollider2D>();
        BackGround.GetComponent<BoxCollider2D>().size = new Vector2(stageData.backgroundWidthSize / PPU, stageData.backgroundHeightSize / PPU);
        BackGround.GetComponent<BoxCollider2D>().offset = new Vector2(0, stageData.backgroundHeightSize / PPU / 2);
        BackGround.GetComponent<BoxCollider2D>().isTrigger = true;
        Ground.transform.localScale = new Vector3(stageData.backgroundWidthSize / PPU / Ground.GetComponent<SpriteRenderer>().size.x, 1f, 1f);

        foreach (var enemy in stageData.enemies)
        {
            for(int i = 0; i < enemy.count; i++)
            {
                GameObject newEnemy = Instantiate(enemy.enemyData.enemyPrefab, 
                    new Vector2(UnityEngine.Random.Range(enemy.enemyData.spawnLeftLimit, enemy.enemyData.spawnRightLimit), 
                    enemy.enemyData.enemyPrefab.transform.position.y),
                    Quaternion.identity);
                EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.OnDeath += EnemyDeath;
                if(enemy.enemyType == EnemyData.Type.Boss)
                {
                    enemyController.OnDeath += UIManager.Instance.ShowGameClearPanel;
                }
                enemyController.InitEnemy(enemy.enemyData);
            }
        }
        GameData.RemainedEnemyInStage = stageData.totalEnemyCount;
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
}
