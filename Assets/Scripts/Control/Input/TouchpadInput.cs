using UnityEngine;

namespace RootCapsule.Control.Input
{
    // Developing: primary, secondary, dragging, pulling
    public class TouchpadInput : PlayerInput
    {
        void Update()
        {
            if (IsPrimaryAction())
            {
                RaisePrimaryPressed();
                return;
            }
            if (IsSecondaryAction())
            {
                RaiseSecondaryPressed();
                return;
            }
        }

        bool IsPrimaryAction()
        {
            return false;
        }

        bool IsSecondaryAction()
        {
            return false;
        }
    }
}