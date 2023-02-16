using RootCapsule.Control.Input;
using UnityEngine;

namespace RootCapsule.Control.SceneControl
{
    [RequireComponent(typeof(InputResolver))]
    public abstract class SceneController : MonoBehaviour
    {
        PlayerInput input;

        void Awake()
        {
            input = gameObject.GetComponent<PlayerInput>();
        }

        void OnEnable()
        {
            input.PrimaryPressed += OnPrimaryPressed;
            input.SecondaryPressed += OnSecondaryPressed;
            input.PullingIn += OnPullingIn;
            input.PullingOut += OnPullingOut;
            input.DraggingStart += OnDraggingStart;
            input.Dragging += OnDragging;
            input.DraggingEnd += OnDraggingEnd;
        }

        void OnDisable()
        {
            input.PrimaryPressed -= OnPrimaryPressed;
            input.SecondaryPressed -= OnSecondaryPressed;
            input.PullingIn -= OnPullingIn;
            input.PullingOut -= OnPullingOut;
            input.DraggingStart -= OnDraggingStart;
            input.Dragging -= OnDragging;
            input.DraggingEnd -= OnDraggingEnd;
        }

        protected virtual void OnDraggingEnd(Vector2 end) { }

        protected virtual void OnDragging(Vector2 newPoint) { }

        protected virtual void OnDraggingStart(Vector2 start) { }

        protected virtual void OnPullingOut(float amount) { }

        protected virtual void OnPullingIn(float amount) { }

        protected virtual void OnSecondaryPressed(Vector2 point) { }

        protected virtual void OnPrimaryPressed(Vector2 point) { }
    }
}