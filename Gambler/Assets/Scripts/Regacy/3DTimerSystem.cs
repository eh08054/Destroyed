using System;
using UnityEngine;

public class My3DTimerSystem
{
    public event Action<float> OnTimeChanged;
    public event Action OnTimeOver;
    private My3DGameData gameData;

    public void Init(My3DGameData gameData)
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
