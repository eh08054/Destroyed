using System.IO;
using UnityEngine;

public class My3DSaveSystem
{
    string path;
    public My3DSaveSystem()
    {
        path = Application.persistentDataPath + "/save.json";
    }
    public void Save(My3DSaveData save) {
        My3DSaveData data = Load();
        if (save.highScore > data.highScore)
        {
            data.highScore = save.highScore;
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
    public My3DSaveData Load() {
        if (!File.Exists(path))
        {
            return new My3DSaveData { highScore = 0 };
        }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<My3DSaveData>(json);
    }
}
