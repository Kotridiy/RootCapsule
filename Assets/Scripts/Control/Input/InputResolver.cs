using UnityEngine;

namespace RootCapsule.Control.Input
{
    // Develop: Check TouchInput
    public class InputResolver : MonoBehaviour
    {
        void Awake()
        {
            gameObject.AddComponent<MouseInput>();
        }
    }
}