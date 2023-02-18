using UnityEngine;
using UnityEngine.UI;

namespace RootCapsule.UI
{
    [RequireComponent(typeof(AudioSource), typeof(Animator))]
    public class MenuButtonController : MonoBehaviour
    {
        public string HoverParamName = "IsHover";
        public AudioClip HoverSound;

        private AudioSource audioSource;
        private Animator animator;

        public void PlayHover()
        {
            audioSource.PlayOneShot(HoverSound);
        }

        public void SetAnimatorHoverParam(bool value)
        {
            animator.SetBool(HoverParamName, value);
        }

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
        }
    }
}