using System.IO;
using TMPro;
using UnityEngine;
public class My3DInitializeScene : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    My3DSaveSystem saveSystem;
    void Start()
    {
        saveSystem = new My3DSaveSystem();
        My3DSaveData saveData = saveSystem.Load();
        text.text += saveData.highScore;
    }
}
