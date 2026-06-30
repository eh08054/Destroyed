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

    [Header("사운드")]
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Slider AllSlider;

    private void Start()
    {
        GraphicButton.onClick.AddListener(ShowGraphicPanel);
        SoundButton.onClick.AddListener(ShowSoundPanel);
        ControlButton.onClick.AddListener(ShowControlPanel);

        BGMSlider.onValueChanged.AddListener(AudioManager.instance.SetBGMVolume);
        SFXSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
        AllSlider.onValueChanged.AddListener(AudioManager.instance.SetAllVolume);
    }
    private void OnDestroy()
    {
        GraphicButton.onClick.RemoveListener(ShowGraphicPanel);
        SoundButton.onClick.RemoveListener(ShowSoundPanel);
        ControlButton.onClick.RemoveListener(ShowControlPanel);

        BGMSlider.onValueChanged.RemoveListener(AudioManager.instance.SetBGMVolume);
        SFXSlider.onValueChanged.RemoveListener(AudioManager.instance.SetSFXVolume);
        AllSlider.onValueChanged.RemoveListener(AudioManager.instance.SetAllVolume);
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
