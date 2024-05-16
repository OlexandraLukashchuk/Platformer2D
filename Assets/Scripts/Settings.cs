using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown QualityDropdown;
    public Toggle FullscreenToggle;
    public Slider MusicSlider;
    public AudioSource audioSource;

    Resolution[] resolutions;

    private void Start()
    {
        ResolutionDropdown.ClearOptions();
        List<string> options = new() { };
        resolutions = Screen.resolutions;
        int currResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currResolutionIndex = i;
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.RefreshShownValue();

        // Load saved volume
        if (PlayerPrefs.HasKey("MusicVolumePreference"))
            MusicSlider.value = PlayerPrefs.GetFloat("MusicVolumePreference");
        else
            MusicSlider.value = 1f; // Default volume

        // Apply the loaded volume
        SetVolume(MusicSlider.value);

        // Connect the SetVolume method to the slider
        MusicSlider.onValueChanged.AddListener(SetVolume);

        LoadSettings(currResolutionIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void ExitSettings()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", QualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", ResolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("MusicVolumePreference", MusicSlider.value);
    }

    public void LoadSettings(int currResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            QualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            QualityDropdown.value = 3;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            ResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            ResolutionDropdown.value = 3;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;

        if (PlayerPrefs.HasKey("MusicVolumePreference"))
            MusicSlider.value = PlayerPrefs.GetFloat("MusicVolumePreference");
        else
            MusicSlider.value = 1f;
        SetVolume(MusicSlider.value);
    }
}