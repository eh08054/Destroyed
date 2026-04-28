using UnityEngine;
using UnityEngine.SceneManagement;

public class My3DSceneChanger : MonoBehaviour
{
    public void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
