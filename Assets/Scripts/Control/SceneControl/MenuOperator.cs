using RootCapsule.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RootCapsule.Control.SceneControl
{
    public class MenuOperator : MonoBehaviour
    {
        public Fader NewGameFader;
        public Fader ExitFader;

        public string FieldsScene;

        private Transform fadersParent;

        public void NewGame()
        {
            StartCoroutine(NewGameCoroutine());
        }

        public void Exit()
        {
            StartCoroutine(ExitCoroutine());
        }

        IEnumerator NewGameCoroutine()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(fadersParent.gameObject);
            NewGameFader.FadeIn();
            yield return new WaitForSeconds(NewGameFader.FadeInDuration);

            SceneManager.LoadScene(FieldsScene);
            yield return new WaitForSeconds(NewGameFader.PauseDuration);

            NewGameFader.FadeOut();
            yield return new WaitForSeconds(NewGameFader.FadeOutDuration);
            Destroy(fadersParent.gameObject);
            Destroy(this);
        }

        IEnumerator ExitCoroutine()
        {
            ExitFader.FadeIn();
            yield return new WaitForSeconds(ExitFader.FadeInDuration);

            SceneManager.LoadScene(FieldsScene);
            Application.Quit();
        }

        private void Awake()
        {
            fadersParent = NewGameFader.GetComponentInParent<Transform>();
        }
    }
}
