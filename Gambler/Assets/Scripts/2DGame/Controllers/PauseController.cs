using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button GameEndButton;

    private void Start()
    {
        backButton.onClick.AddListener(BackGame);
        newGameButton.onClick.AddListener(NewGame);
        SettingsButton.onClick.AddListener(OpenSettings);
        GameEndButton.onClick.AddListener(EndGame);
    }
    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(BackGame);
        newGameButton.onClick.RemoveListener(NewGame);
        SettingsButton.onClick.RemoveListener(OpenSettings);
        GameEndButton.onClick.RemoveListener(EndGame);
    }

    private void BackGame()
    {
        UIManager.Instance.ClosePanel(gameObject);
    }
    private void NewGame()
    {
        GameManager.Instance.SceneChanger.SceneChange("MapScene");
    }
    private void OpenSettings()
    {
        UIManager.Instance.OpenPanel(GameManager.Instance.SettingsPanel);
    }
    private void EndGame()
    {
        UIManager.Instance.OpenPanel(GameManager.Instance.GameEndPanel);
    }
}
