using UnityEngine;
using UnityEngine.SceneManagement;
public class StageManager : MonoBehaviour
{
    public void OnClickStage(int x)
    {
        GameData.SelectedStage = x;
        SceneManager.LoadScene("GameScene");
    }
}
