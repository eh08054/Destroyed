using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    private Resolution[] resolutions;
    private int currentResolutionIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Init(TMP_Dropdown screenDropdown, TMP_Dropdown ResolutionDropdown)
    {
        screenDropdown.ClearOptions();
        List<string> screenOptions = new List<string>
        {
            "전체화면",
            "창 모드"
        };
        screenDropdown.AddOptions(screenOptions);


        ResolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        currentResolutionIndex = 0;

        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(new List<string>(resolutionOptions));
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

        SetScreenMode(screenDropdown.value);
    }
    public void SetScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(resolutions[currentResolutionIndex].width,
                    resolutions[currentResolutionIndex].height,
                    FullScreenMode.FullScreenWindow);
                break;
            case 1:
                Screen.SetResolution(resolutions[currentResolutionIndex].width,
                    resolutions[currentResolutionIndex].height,
                    FullScreenMode.Windowed);
                break;
        }
    }
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        currentResolutionIndex = index;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
