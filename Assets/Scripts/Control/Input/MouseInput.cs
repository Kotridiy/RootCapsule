using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace RootCapsule.Control.Input
{
    // Developing: Dragging, Pulling
    public class MouseInput : PlayerInput
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
            if (UnityInput.GetMouseButtonDown(0))
            {
                point = UnityInput.mousePosition;
                return true;
            }
            return false;
        }

        bool IsSecondaryAction(ref Vector2 point)
        {
            if (UnityInput.GetMouseButtonDown(1))
            {
                point = UnityInput.mousePosition;
                return true;
            }
            return false;
        }
    }
}