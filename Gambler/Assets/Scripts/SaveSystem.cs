using System.IO;
using UnityEngine;

public class SaveSystem
{
    string path;
    public SaveSystem()
    {
        path = Application.persistentDataPath + "/save.json";
    }
    public void Save(SaveData save) {
        SaveData data = Load();
        if (save.highScore > data.highScore)
        {
            data.highScore = save.highScore;
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
    public SaveData Load() {
        if (!File.Exists(path))
        {
            return new SaveData { highScore = 0 };
        }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
