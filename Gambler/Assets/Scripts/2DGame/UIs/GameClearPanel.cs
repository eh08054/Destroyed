using UnityEngine;
using UnityEngine.UI;

public class GameClearPanel : MonoBehaviour
{
    [SerializeField] private Button ShelterB;
    [SerializeField] private Button MenuB;
    private void Start()
    {
        MenuB.onClick.AddListener(() => GameManager.Instance.SceneChanger.SceneChange("MenuScene"));
        ShelterB.onClick.AddListener(() => GameManager.Instance.SceneChanger.SceneChange("MapScene"));
        gameObject.SetActive(false);
    }
}
