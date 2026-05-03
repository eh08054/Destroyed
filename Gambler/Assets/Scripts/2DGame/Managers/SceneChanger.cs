using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
