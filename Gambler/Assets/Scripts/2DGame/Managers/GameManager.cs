using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public GameData GameData { get; private set; }
    public GameObject BackGround { get; private set; }
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject dragonPrefab;
    [SerializeField] private GameObject orgePrefab;
    [SerializeField] private GameObject backgroundPrefab;
    private void Awake()
    {
        Instance = this;
        Player = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        BackGround = Instantiate(backgroundPrefab, backgroundPrefab.transform.position, Quaternion.identity);
        GameData = new GameData();
    }
    private void Start()
    {
        PlayerController playerController = Player.GetComponent<PlayerController>();
        SpawnEnemies();
    }
    public void SpawnEnemies()
    {
        SpawnElite();
        //SpawnBoss();
    }
    public void SpawnElite()
    {
        GameObject BossEnemy = Instantiate(orgePrefab, orgePrefab.transform.position, Quaternion.identity);
        EnemyController enemyController = BossEnemy.GetComponent<EnemyController>();
        enemyController.OnDeath += EnemyDeath;
        if (enemyController.Enemy is Dragon)
        {
            enemyController.OnDeath += UIManager.Instance.ShowGameClearPanel;
        }
    }
    public void SpawnBoss()
    {
        GameObject BossEnemy = Instantiate(dragonPrefab, dragonPrefab.transform.position, Quaternion.identity);
        EnemyController enemyController = BossEnemy.GetComponent<EnemyController>();
        enemyController.OnDeath += EnemyDeath;
        if (enemyController.Enemy is Dragon)
        {
            enemyController.OnDeath += UIManager.Instance.ShowGameClearPanel;
        }
    }
    public void StopTime()
    {
        Time.timeScale = 0f;
    }
    public void EnemyDeath()
    {
        //Add Score... 
    }
}
