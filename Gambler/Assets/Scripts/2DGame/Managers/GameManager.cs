using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public GameObject BackGround { get; private set; }
    public GameData GameData { get; private set; }
    [SerializeField] private List<StageData> stages;
    [SerializeField] private GameObject playerPrefab;

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
        BackGround.GetComponent<BoxCollider2D>().size = new Vector2(stageData.backgroundWidthSize / 32, stageData.backgroundHeightSize / 32);
        BackGround.GetComponent<BoxCollider2D>().offset = Vector2.zero;
        BackGround.GetComponent<BoxCollider2D>().isTrigger = true;

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
