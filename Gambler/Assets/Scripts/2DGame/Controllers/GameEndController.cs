using UnityEngine;
using UnityEngine.UI;

public class GameEndController : MonoBehaviour
{
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;

    private void Start()
    {
        YesButton.onClick.AddListener(GameEnd);
        NoButton.onClick.AddListener(Close);
        gameObject.SetActive(false);
    }

    private void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Close()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ClosePanel(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
