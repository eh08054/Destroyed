using UnityEngine;

public class My3DGameData
{
    public int currentScore;
    public float timer;
    public void Init(int currentScore, float timer)
    {
        this.currentScore = currentScore;
        this.timer = timer;
    }
    public My3DSaveData ToSaveData()
    {
        return new My3DSaveData { highScore = currentScore};
    }
}
