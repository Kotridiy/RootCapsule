using UnityEngine;

namespace RootCapsule.Control.Input
{
    // Develop: Check TouchInput
    public class InputResolver : MonoBehaviour
    {
        void Awake()
        {
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                gameObject.AddComponent<MouseInput>();
            }
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                gameObject.AddComponent<TouchpadInput>();
            }
        }
    }
}