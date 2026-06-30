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
    [SerializeField] private GameObject SettingsCanvas;

    private SaveData saveData;
    private SettingsManager settings;

    private void Start()
    {
        saveData = SaveSystem.Load();

        settings = SettingsCanvas.GetComponent<SettingsManager>();
        settings.AllSlider.value = saveData.AllVolume;
        settings.BGMSlider.value = saveData.BGMVolume;
        settings.SFXSlider.value = saveData.SFXVolume;

        AudioManager.instance.SetAllVolume(saveData.AllVolume);
        AudioManager.instance.SetBGMVolume(saveData.BGMVolume);
        AudioManager.instance.SetSFXVolume(saveData.SFXVolume);
        AudioManager.instance.PlayBGM(BGM.TITLE);

        StartCoroutine(TextBlink());
        EndButton.onClick.AddListener(EndGame);
        SettingsButton.onClick.AddListener(OpenSettingPanel);
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
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                SceneManager.LoadScene("MapScene");
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

        SaveSystem.Save(saveData);
    }

    private IEnumerator TextBlink()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));
            yield return StartCoroutine(Fade(0, 1));
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = AnyKeyText.GetComponent<TMP_Text>().color;
            color.a = Mathf.Lerp(start, end, percent);
            AnyKeyText.GetComponent<TMP_Text>().color = color;

            yield return null;
        }
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OpenSettingPanel()
    {
        SettingsCanvas.SetActive(true);
    }
}
