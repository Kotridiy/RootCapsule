using UnityEngine;

namespace RootCapsule.Control.Input
{
    // Develop: Check TouchInput
    public class InputResolver : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.AddComponent<MouseInput>();
        }
    }
}