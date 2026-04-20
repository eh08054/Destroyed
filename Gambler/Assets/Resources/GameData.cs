using UnityEngine;

public class GameData
{
    public int currentScore;
    public float timer;
    public void Init(int currentScore, float timer)
    {
        this.currentScore = currentScore;
        this.timer = timer;
    }
    public SaveData ToSaveData()
    {
        return new SaveData{highScore = currentScore};
    }
}
