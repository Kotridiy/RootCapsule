using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace RootCapsule.Control.Input
{
    // Developing: primary, secondary, dragging, pulling
    public class TouchpadInput : PlayerInput
    {
        void Update()
        {
            Vector2 point = new Vector2();
            if (IsPrimaryAction(ref point))
            {
                RaisePrimaryPressed(point);
                return;
            }
            if (IsSecondaryAction(ref point))
            {
                RaiseSecondaryPressed(point);
                return;
            }
        }

        bool IsPrimaryAction(ref Vector2 point)
        {
            foreach (var touch in UnityInput.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    point = touch.position;
                    return true;
                }
            }
            return false;
        }

        bool IsSecondaryAction(ref Vector2 point)
        {
            return false;
        }
    }
}