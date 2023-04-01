using RootCapsule.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RootCapsule.Control.SceneControl
{
    public class MainMenuOperator : MonoBehaviour
    {
        public Fader StartGameFader;
        public Fader BlueFader;
        public Fader ExitFader;

        [Space(10)]
        public string StartGameSceneName;
        public string SettingsSceneName;

        public void StartGame()
        {
            StartGameFader.FadingChangeScene(StartGameSceneName);
        }

        public void OpenSettings()
        {
            BlueFader.FadingChangeScene(SettingsSceneName);
        }

        public void Exit()
        {
            StartCoroutine(ExitCoroutine());
        }

        IEnumerator ExitCoroutine()
        {
            ExitFader.FadeIn();
            yield return new WaitForSeconds(ExitFader.FadeInDuration);

            Application.Quit();
        }
    }
}
