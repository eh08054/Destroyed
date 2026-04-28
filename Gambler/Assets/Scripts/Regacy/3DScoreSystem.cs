using UnityEngine;
using System;
public class My3DScoreSystem
{
    private My3DGameData gameData;
    public event Action<int> OnScoreChanged;
    public void Init(My3DGameData gameData)
    {
        this.gameData = gameData;
    }
    public void AddScore(int amount)
    {
        gameData.currentScore += amount;
        OnScoreChanged?.Invoke(gameData.currentScore);
    }
}
