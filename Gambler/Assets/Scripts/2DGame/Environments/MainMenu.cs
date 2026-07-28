using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject AnyKeyText;
    [SerializeField] private Button EndButton;
    [SerializeField] private Button SettingsButton;

    [SerializeField] private float fadeTime;
    [SerializeField] private float SceneFadeTime;
    [SerializeField] private GameObject GameEndCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject FadePanel;

    private SaveData saveData;
    private SettingsController settings;
    private GraphicBlink graphicBlink;

    private TMP_Text anyKeyText;
    private Image fadeImage;

    private void Start()
    {
        saveData = SaveSystem.Load();

        settings = SettingsCanvas.GetComponent<SettingsController>();
        settings.AllSlider.value = saveData.AllVolume;
        settings.BGMSlider.value = saveData.BGMVolume;
        settings.SFXSlider.value = saveData.SFXVolume;
        settings.HUDSlider.value = saveData.HUDAlpha;

        AudioManager.instance.SetAllVolume(saveData.AllVolume);
        AudioManager.instance.SetBGMVolume(saveData.BGMVolume);
        AudioManager.instance.SetSFXVolume(saveData.SFXVolume);
        AudioManager.instance.PlayBGM(BGM.TITLE);

        EndButton.onClick.AddListener(OpenGameEndPanel);
        SettingsButton.onClick.AddListener(OpenSettingPanel);

        anyKeyText = AnyKeyText.GetComponent<TMP_Text>();
        fadeImage = FadePanel.GetComponent<Image>();
        graphicBlink = new GraphicBlink();
        StartCoroutine(TextBlink());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SettingsCanvas.activeSelf)
                {
                    SettingsCanvas.SetActive(false);
                }
                else if (GameEndCanvas.activeSelf)
                {
                    GameEndCanvas.SetActive(false);
                }
                else
                {
                    GameEndCanvas.SetActive(true);
                }
                
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                FadePanel.SetActive(true);
                StartCoroutine(FadeScene());
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EndButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.RemoveAllListeners();

        saveData.AllVolume = settings.AllSlider.value;
        saveData.BGMVolume = settings.BGMSlider.value;
        saveData.SFXVolume = settings.SFXSlider.value;
        saveData.HUDAlpha = settings.HUDSlider.value;

        SaveSystem.Save(saveData);
    }
    private IEnumerator FadeScene()
    {
        yield return StartCoroutine(graphicBlink.FadeGraphic(fadeImage, 0, 1, SceneFadeTime));
        SceneManager.LoadScene("MapScene");
    }
    private IEnumerator TextBlink()
    {
        while (true)
        {
            yield return StartCoroutine(graphicBlink.FadeGraphic(anyKeyText, 1, 0, fadeTime));
            yield return StartCoroutine(graphicBlink.FadeGraphic(anyKeyText, 0, 1, fadeTime));
        }
    }
    
    private void OpenSettingPanel()
    {
        SettingsCanvas.SetActive(true);
    }
    private void OpenGameEndPanel()
    {
        GameEndCanvas.SetActive(true);
    }
}
