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
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(BackGame);
        newGameButton.onClick.RemoveListener(NewGame);
        SettingsButton.onClick.RemoveListener(OpenSettings);
        GameEndButton.onClick.RemoveListener(EndGame);
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            Time.timeScale = 0f;
            GameManager.Instance.gameState = GameState.Pausing;
        }
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
        GameManager.Instance.gameState = GameState.Playing;
    }

    private void BackGame()
    {
        UIManager.Instance.ClosePanel(gameObject);
    }
    private void NewGame()
    {
        GameManager.Instance.SceneChanger.SceneChange("MapScene");
        UIManager.Instance.ClosePanel(gameObject);
    }
    private void OpenSettings()
    {
        UIManager.Instance.OpenPanel(UIManager.Instance.SettingsPanel);
    }
    private void EndGame()
    {
        UIManager.Instance.OpenPanel(UIManager.Instance.GameEndPanel);
    }
}
