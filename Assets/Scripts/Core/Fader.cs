using System.Collections;
using UnityEngine;

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

        void Awake()
        {
            animator = GetComponent<Animator>();    
        }
    }
}