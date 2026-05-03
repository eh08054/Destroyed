using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Slider PlayerHPSlider;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PlayerController playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        playerController.OnBirth += HPSliderInit;
        playerController.OnHPChanged += SetPlayerHP;
        playerController.OnDeath += ShowGameOverPanel;
    }
    public void HPSliderInit(int max)
    {
        PlayerHPSlider.maxValue = max;
        PlayerHPSlider.value = PlayerHPSlider.maxValue;
        HPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;
    }
    public void SetPlayerHP(int current)
    {
        PlayerHPSlider.value = current;
        HPText.text = PlayerHPSlider.value + "/" + PlayerHPSlider.maxValue;
    }
    public void ShowGameClearPanel()
    {
        StartCoroutine("ShowGamePanelDelay");
    }
    private IEnumerator ShowGamePanelDelay()
    {
        yield return new WaitForSeconds(0.5f);
        gameClearPanel.SetActive(true);
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
    private void OnDestory()
    {
        PlayerController playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        playerController.OnBirth -= HPSliderInit;
        playerController.OnHPChanged -= SetPlayerHP;
    }
}
