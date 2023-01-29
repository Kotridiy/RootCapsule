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

        protected virtual void OnDraggingEnd(Vector2 obj) { }

        protected virtual void OnDragging(Vector2 obj) { }

        protected virtual void OnDraggingStart(Vector2 obj) { }

        protected virtual void OnPullingOut(float obj) { }

        protected virtual void OnPullingIn(float obj) { }

        protected virtual void OnSecondaryPressed() { }

        protected virtual void OnPrimaryPressed() { }
    }
}