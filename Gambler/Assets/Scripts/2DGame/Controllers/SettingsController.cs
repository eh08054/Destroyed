using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
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

    [Header("그래픽")]
    public TMP_Dropdown ScreenDropDown;
    public TMP_Dropdown ResDropDown;

    [Header("게임 플레이")]
    public Slider HUDSlider;

    [Header("공통")]
    public TMP_Text SettingType;

    private void Start()
    {
        GraphicButton.onClick.AddListener(ShowGraphicPanel);
        SoundButton.onClick.AddListener(ShowSoundPanel);
        ControlButton.onClick.AddListener(ShowControlPanel);

        BGMSlider.onValueChanged.AddListener(AudioManager.instance.SetBGMVolume);
        SFXSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
        AllSlider.onValueChanged.AddListener(AudioManager.instance.SetAllVolume);

        ScreenManager.instance.Init(ScreenDropDown, ResDropDown);
        ScreenDropDown.onValueChanged.AddListener(ScreenManager.instance.SetScreenMode);
        ResDropDown.onValueChanged.AddListener(ScreenManager.instance.SetResolution);

        HUDSlider.onValueChanged.AddListener(OnHUDAlphaChanged);
        gameObject.SetActive(false);
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

    public void OnHUDAlphaChanged(float value)
    {
        if(UIManager.Instance == null) { return; }
        UIManager.Instance.SetHUDAlpha(value);
        if(GameManager.Instance == null) { return; }
        GameManager.Instance.GameData.HUDAlpha = value;
    }

    private void ShowGraphicPanel()
    {
        GraphicPanel.SetActive(true);
        SoundPanel.SetActive(false);
        ControlPanel.SetActive(false);
        SettingType.text = "그래픽 설정";
    }
    private void ShowSoundPanel()
    {
        GraphicPanel.SetActive(false);
        SoundPanel.SetActive(true);
        ControlPanel.SetActive(false);
        SettingType.text = "사운드 설정";
    }
    private void ShowControlPanel()
    {
        GraphicPanel.SetActive(false);
        SoundPanel.SetActive(false);
        ControlPanel.SetActive(true);
        SettingType.text = "게임 플레이 설정";
    }
}
