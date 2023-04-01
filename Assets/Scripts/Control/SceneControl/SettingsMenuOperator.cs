using RootCapsule.Core;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RootCapsule.Control.SceneControl
{
    public class SettingsMenuOperator : MonoBehaviour
    {
        const string SETTINGS_KEY = "SettingsSet";
        const string WINDOW_REZOLUTION_KEY = "WindowResolution";
        const string LANGUAGE_KEY = "Language";
        const string FULLSCREEN_KEY = "Fullscreen";
        const string MUSIC_VOLUME_KEY = "MusicVolum";
        const string SOUND_VOLUME_KEY = "SoundVolume";

        public Dropdown WindowResolutionDropdown;
        public Dropdown LanguageDropdown;
        public Toggle FullscreenToggle;
        public Slider MusicVolumeSlider;
        public Slider SoundVolumeSlider;

        [Space(10)]
        public Animator ConfirmationWindowAnimator;
        public Fader BackFader;

        [Space(10)]
        public string MainMenuSceneName;

        public static void SetDefaultSettings()
        {
            if (!PlayerPrefs.HasKey(SETTINGS_KEY))
            {
                PlayerPrefs.SetInt(SETTINGS_KEY, 1);
                PlayerPrefs.SetInt(WINDOW_REZOLUTION_KEY, 0);
                PlayerPrefs.SetInt(LANGUAGE_KEY, 0);
                PlayerPrefs.SetInt(FULLSCREEN_KEY, 0);
                PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, 0.5f);
                PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, 0.5f);

                PlayerPrefs.Save();
            }
        }

        public void OnWindowResolutionChange()
        {
            var resolution = WindowResolutionDropdown.options[WindowResolutionDropdown.value].text;
            var splitedResolution = resolution.Split('x');
            Screen.SetResolution(int.Parse(splitedResolution[0]), int.Parse(splitedResolution[1]), Screen.fullScreen);

            PlayerPrefs.SetInt(WINDOW_REZOLUTION_KEY, WindowResolutionDropdown.value);
        }

        public void OnLanguageChange()
        {
            PlayerPrefs.SetInt(LANGUAGE_KEY, LanguageDropdown.value);
        }

        public void OnFullscreenChange()
        {
            Screen.fullScreen = FullscreenToggle.isOn;
            PlayerPrefs.SetInt(FULLSCREEN_KEY, FullscreenToggle.isOn ? 1 : 0);
        }

        public void OnMusicVolumeChange()
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolumeSlider.value);
        }

        public void OnSoundVolumeChange()
        {
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, SoundVolumeSlider.value);
        }

        public void ShowConfirmationWindow()
        {
            ConfirmationWindowAnimator.SetBool("ShowConfirmation", true);
        }

        public void HideConfirmationWindow()
        {
            ConfirmationWindowAnimator.SetBool("ShowConfirmation", false);
        }

        public void OnEraseProgress()
        {
            PlayerPrefs.DeleteKey(SETTINGS_KEY);
            SetDefaultSettings();
            GetSavedSettings();

            OnWindowResolutionChange();
            OnLanguageChange();
            OnFullscreenChange();
            OnMusicVolumeChange();
            OnSoundVolumeChange();

            SceneManager.LoadScene(0);
        }

        public void OnExit()
        {
            BackFader.FadingChangeScene(MainMenuSceneName);
        }

        enum LanguageResolution
        {
            Russian,
            English
        }

        private void Start()
        {
            WindowResolutionDropdown.options.Clear();
            foreach (var res in Screen.resolutions)
            {
                string resStr = res.width + "x" + res.height;
                if (!WindowResolutionDropdown.options.Any(opt => opt.text == resStr))
                {
                    WindowResolutionDropdown.options.Add(new Dropdown.OptionData(resStr));
                }
            }
            WindowResolutionDropdown.options.Reverse();

            SetDefaultSettings();
            GetSavedSettings();
        }

        private void OnDisable()
        {
            PlayerPrefs.Save();
        }

        void GetSavedSettings()
        {
            // Window Resolution
            if (PlayerPrefs.HasKey(WINDOW_REZOLUTION_KEY))
            {
                WindowResolutionDropdown.value = PlayerPrefs.GetInt(WINDOW_REZOLUTION_KEY);
            }

            // Language
            if (PlayerPrefs.HasKey(LANGUAGE_KEY))
            {
                LanguageDropdown.value = PlayerPrefs.GetInt(LANGUAGE_KEY);
            }

            // Fullscreen
            if (PlayerPrefs.HasKey(FULLSCREEN_KEY))
            {
                FullscreenToggle.isOn = PlayerPrefs.GetInt(FULLSCREEN_KEY) > 0;
            }

            // Music Volume
            if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
            {
                MusicVolumeSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
            }

            // Sound Volume
            if (PlayerPrefs.HasKey(SOUND_VOLUME_KEY))
            {
                SoundVolumeSlider.value = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY);
            }
        }
    }
}