using System;
using UnityEngine;

public class TimerSystem
{
    public event Action<float> OnTimeChanged;
    public event Action OnTimeOver;
    private GameData gameData;

    public void Init(GameData gameData)
    {
        this.gameData = gameData;
    }
    public void Tick(float deltaTime)
    {
        gameData.timer -= deltaTime;

        OnTimeChanged?.Invoke(gameData.timer);

        if(gameData.timer <= 0)
        {
            gameData.timer = 0;
            OnTimeOver?.Invoke();
        }
    }
}
