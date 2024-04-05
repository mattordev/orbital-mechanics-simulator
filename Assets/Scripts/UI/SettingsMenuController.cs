using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using Mattordev.Utils.Audio;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class SettingsMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private AudioMixer masterMixer;

        // Graphics
        [SerializeField] private TMP_Dropdown graphicsDropdown;
        private Resolution[] resolutions;
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        [Header("Elements")]
        public Toggle fullscreenToggle;
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider SFXSlider;
        public Slider UISlider;


        private void Start()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResoltionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = $"{resolutions[i].width}x{resolutions[i].height}@{resolutions[i].refreshRate}Hz";

                options.Add(option);

                // Unity wont let me compare two resolutions directly, so we have to compare the WIDTH, then the HEIGHT.
                if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResoltionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResoltionIndex;
            resolutionDropdown.RefreshShownValue();

            QualitySettings.SetQualityLevel(graphicsDropdown.value);
            LoadSettings();

            settingsMenu.gameObject.SetActive(false);
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

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        #region  Audio

        public void SetVolumeMaster(float volume)
        {
            masterMixer.SetFloat("Master Volume", Mathf.Log(volume) * 20);
        }

        public void SetVolumeMusic(float volume)
        {
            masterMixer.SetFloat("Music Volume", Mathf.Log(volume) * 20);
        }

        public void SetVolumeSFX(float volume)
        {
            masterMixer.SetFloat("SFX Volume", Mathf.Log(volume) * 20);
        }

        public void SetVolumeUI(float volume)
        {
            masterMixer.SetFloat("UI Volume", Mathf.Log(volume) * 20);
        }

        #endregion

        #region SAVING

        public void SaveAudio(string name, float volume)
        {
            PlayerPrefs.SetFloat(name, Mathf.Log(volume) * 20);
        }

        public void LoadSettings()
        {
            // Set the quality
            int quality = PlayerPrefs.GetInt("quality", 3);
            QualitySettings.SetQualityLevel(quality);
            graphicsDropdown.value = quality;
            // set the resolution
            int resolution = PlayerPrefs.GetInt("resolution", 0);
            resolutionDropdown.value = resolution;
            SetResolution(resolution);
            // Set fullscreen
            int fullscreen = PlayerPrefs.GetInt("fullscreen", 1);
            fullscreenToggle.isOn = fullscreen == 1;
            SetFullscreen(fullscreen == 1);

            // Set the sliders to the correct place
            float master = PlayerPrefs.GetFloat("masterVolume", 1f);
            float music = PlayerPrefs.GetFloat("musicVolume", 1f);
            float sfx = PlayerPrefs.GetFloat("sfxVolume", 1f);
            float ui = PlayerPrefs.GetFloat("uiVolume", 1f);

            // Set slider values
            masterSlider.value = master;
            musicSlider.value = music;
            SFXSlider.value = sfx;
            UISlider.value = ui;

            // Set the mixers with the clamped volume values
            masterMixer.SetFloat("Master Volume", master);
            masterMixer.SetFloat("Music Volume", music);
            masterMixer.SetFloat("SFX Volume", sfx);
            masterMixer.SetFloat("UI Volume", ui);
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
            PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
            PlayerPrefs.SetInt("resolution", resolutionDropdown.value);
            PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
            PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
            PlayerPrefs.SetFloat("sfxVolume", SFXSlider.value);
            PlayerPrefs.SetFloat("uiVolume", UISlider.value);
        }

        [ContextMenu("Clear PlayerPrefs")]
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }



        #endregion
    }
}
