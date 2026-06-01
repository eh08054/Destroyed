using UnityEngine;
using System.IO;
public class SaveSystem
{
    public static void Save(SaveData saveData)
    {
        string path = Application.persistentDataPath + "/saveData.json";
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }
    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/saveData.json";
        if (!File.Exists(path)) { return new SaveData(); }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
