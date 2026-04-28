using UnityEngine;
using TMPro;
using System.Collections;

public class My3DUIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timeText;
    private My3DScoreSystem scoreSystem;
    private My3DTimerSystem timerSystem;
    private My3DGameManager gameManager;
    [SerializeField] private GameObject RestartPanel;
    public void Init(My3DGameContext context)
    {
        scoreSystem = context.scoreSystem;
        timerSystem = context.timerSystem;
        gameManager = context.gameManager;
    }
    public void Start()
    {
        scoreSystem.OnScoreChanged += UpdateScoreUI;
        timerSystem.OnTimeChanged += UpdateTimeUI;
        gameManager.OnGameEnd += GameEndUI;
    }
    public void UpdateScoreUI(int score)
    {
        scoreText.text = "SCORE: " + score.ToString();
    }
    public void UpdateTimeUI(float time)
    {
        timeText.text = "TIME: " + time.ToString("F2");
    }
    public void GameEndUI()
    {
        RestartPanel.SetActive(true);
    }
}
