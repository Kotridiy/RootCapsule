using UnityEngine;
using UnityEngine.UI;

namespace RootCapsule.UI
{
    [RequireComponent(typeof(Animator))]
    public class MenuButtonController : MonoBehaviour
    {
        public string HoverParamName = "IsHover";
        private Animator animator;

        public void PlayHover()
        {
        }

        public void SetAnimatorHoverParam(bool value)
        {
            animator.SetBool(HoverParamName, value);
        }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
    }
}