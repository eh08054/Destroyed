using UnityEngine;
using System;
public class ScoreSystem
{
    private GameData gameData;
    public event Action<int> OnScoreChanged;
    public void Init(GameData gameData)
    {
        this.gameData = gameData;
    }
    public void AddScore(int amount)
    {
        gameData.currentScore += amount;
        OnScoreChanged?.Invoke(gameData.currentScore);
    }
}
