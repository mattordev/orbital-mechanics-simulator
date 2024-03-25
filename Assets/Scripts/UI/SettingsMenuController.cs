using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

/// <author>
/// Authored & Written by @mattordev
/// 
/// for external use, please contact the author directly
/// </author>
namespace Mattordev.UI
{
    public class SettingsMenuController : MonoBehaviour
    {
        [SerializeField] private AudioMixer masterMixer;

        // Graphics
        [SerializeField] private TMP_Dropdown graphicsDropdown;
        private Resolution[] resolutions;
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        private void Start()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResoltionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
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
            this.gameObject.SetActive(false);
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
            masterMixer.SetFloat("Master Volume", volume);
        }

        public void SetVolumeMusic(float volume)
        {
            masterMixer.SetFloat("Music Volume", volume);
        }

        public void SetVolumeSFX(float volume)
        {
            masterMixer.SetFloat("SFX Volume", volume);
        }

        public void SetVolumeUI(float volume)
        {
            masterMixer.SetFloat("UI Volume", volume);
        }

        #endregion

    }
}
