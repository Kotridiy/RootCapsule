using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RootCapsule.Core
{
    [RequireComponent(typeof(Animator))]
    public class Fader : MonoBehaviour
    {

        public float FadeInDuration = 1;
        public float FadeOutDuration = 1;
        public float PauseDuration = 0.5f;

        [SerializeField] string FadeInTrigger = "FadeIn";
        [SerializeField] string FadeOutTrigger = "FadeOut";

        Animator animator;

        public void FadeIn()
        {
            gameObject.SetActive(true);
            animator.SetTrigger(FadeInTrigger);
            animator.speed = 1 / FadeInDuration;
        }

        public void FadeOut()
        {
            animator.SetTrigger(FadeOutTrigger);
            animator.speed = 1 / FadeOutDuration;
        }

        public void FadingChangeScene(string sceneName)
        {
            StartCoroutine(FadingChangeSceneCoroutine(sceneName));
        }

        void Awake()
        {
            animator = GetComponent<Animator>();    
        }

        IEnumerator FadingChangeSceneCoroutine(string sceneName)
        {
            Transform saved = transform.parent ?? transform;
            DontDestroyOnLoad(saved.gameObject);
            FadeIn();
            yield return new WaitForSeconds(FadeInDuration);

            SceneManager.LoadScene(sceneName);
            yield return new WaitForSeconds(PauseDuration);

            FadeOut();
            yield return new WaitForSeconds(FadeOutDuration);
            Destroy(saved.gameObject);
        }
    }
}