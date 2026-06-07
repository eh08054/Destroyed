#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneChangeEditor : Editor
{
    [MenuItem("SceneMove/MenuScene &1")]
    private static void MainScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/2DGame/MenuScene.unity");
    }

    [MenuItem("SceneMove/MapScene &2")]
    private static void MapScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/2DGame/MapScene.unity");
    }

    [MenuItem("SceneMove/GameScene &3")]
    private static void GameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/2DGame/GameScene.unity");
    }
}
#endif
