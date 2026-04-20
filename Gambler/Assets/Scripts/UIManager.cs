using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timeText;
    private ScoreSystem scoreSystem;
    private TimerSystem timerSystem;
    public void Init(GameContext context)
    {
        scoreSystem = context.scoreSystem;
        timerSystem = context.timerSystem;
    }
    public void Start()
    {
        scoreSystem.OnScoreChanged += UpdateScoreUI;
        timerSystem.OnTimeChanged += UpdateTimeUI;
    }
    public void UpdateScoreUI(int score)
    {
        scoreText.text = "SCORE: " + score.ToString();
    }
    public void UpdateTimeUI(float time)
    {
        timeText.text = "TIME: " + time.ToString("F2");
    }
}
