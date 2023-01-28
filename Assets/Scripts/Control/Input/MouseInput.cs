using UnityEngine;

namespace RootCapsule.Control.Input
{
    // Developing: Dragging, Pulling
    public class MouseInput : PlayerInput
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
            if (UnityEngine.Input.GetMouseButtonDown(0))
                return true;
            return false;
        }

        bool IsSecondaryAction()
        {
            if (UnityEngine.Input.GetMouseButtonDown(1))
                return true;
            return false;
        }
    }
}