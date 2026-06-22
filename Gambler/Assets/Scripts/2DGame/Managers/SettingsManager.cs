using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button GraphicButton;
    [SerializeField] private Button SoundButton;
    [SerializeField] private Button ControlButton;

    [Header("패널")]
    [SerializeField] private GameObject GraphicPanel;
    [SerializeField] private GameObject SoundPanel;
    [SerializeField] private GameObject ControlPanel;

    private void Start()
    {
        GraphicButton.onClick.AddListener(ShowGraphicPanel);
        SoundButton.onClick.AddListener(ShowSoundPanel);
        ControlButton.onClick.AddListener(ShowControlPanel);
    }
    private void OnDestroy()
    {
        GraphicButton.onClick.RemoveListener(ShowGraphicPanel);
        SoundButton.onClick.RemoveListener(ShowSoundPanel);
        ControlButton.onClick.RemoveListener(ShowControlPanel);
    }

    private void ShowGraphicPanel()
    {
        GraphicPanel.SetActive(true);
        SoundPanel.SetActive(false);
        ControlPanel.SetActive(false);
    }
    private void ShowSoundPanel()
    {
        GraphicPanel.SetActive(false);
        SoundPanel.SetActive(true);
        ControlPanel.SetActive(false);
    }
    private void ShowControlPanel()
    {
        GraphicPanel.SetActive(false);
        SoundPanel.SetActive(false);
        ControlPanel.SetActive(true);
    }
}
