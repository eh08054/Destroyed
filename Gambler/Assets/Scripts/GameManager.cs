using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public GameContext context;
    private TimerSystem timerSystem;
    private SaveSystem saveSystem;
    private GameData gameData;
    public GameObject cubePrefab;
    public int scoreInit = 0;
    public float timeInit = 10f;
    [SerializeField] private PlayerController player;
    [SerializeField] private UIManager ui;
    [SerializeField] private SceneChanger sceneChanger;

    private void Awake()
    {
        saveSystem = new SaveSystem();
        gameData = new GameData();
        gameData.Init(scoreInit, timeInit);
        context = new GameContext
        {
            scoreSystem = new ScoreSystem(),
            timerSystem = new TimerSystem()
        };
        context.scoreSystem.Init(gameData);
        context.timerSystem.Init(gameData);
        player.Init(context);
        ui.Init(context);
    }
    public void Start()
    {
        timerSystem = context.timerSystem;
        timerSystem.OnTimeOver += TimeOver;
        for (int i = 0; i < 100; i++)
        {
            GameObject cubeInstance = Instantiate(cubePrefab);
            float x = UnityEngine.Random.Range(-100, 100);
            float z = UnityEngine.Random.Range(-100, 100);
            cubeInstance.transform.Translate(x, transform.localScale.y / 2, z);
        }
    } 
    public void Update()
    {
        timerSystem.Tick(Time.deltaTime);
    }
    public void TimeOver()
    {
        SaveData saveData = gameData.ToSaveData();
        saveSystem.Save(saveData);
        sceneChanger.SceneChange("MenuScene");
    }
}
