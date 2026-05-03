using UnityEngine;
using System;
public class My3DGameManager : MonoBehaviour
{
    public My3DGameContext context;
    private My3DTimerSystem timerSystem;
    private My3DSaveSystem saveSystem;
    private My3DGameData gameData;
    public GameObject cubePrefab;
    public int scoreInit = 0;
    public float timeInit = 10f;
    [SerializeField] private My3DPlayerController player;
    [SerializeField] private My3DUIManager ui;
    public event Action OnGameEnd;

    private void Awake()
    {
        saveSystem = new My3DSaveSystem();
        gameData = new My3DGameData();
        gameData.Init(scoreInit, timeInit);
        context = new My3DGameContext
        {
            scoreSystem = new My3DScoreSystem(),
            timerSystem = new My3DTimerSystem(),
            gameManager = this
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
        OnGameEnd?.Invoke();
        My3DSaveData saveData = gameData.ToSaveData();
        saveSystem.Save(saveData);
    }
}
