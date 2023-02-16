﻿using System;
using UnityEngine;

namespace RootCapsule.Control.Input
{
    public abstract class PlayerInput : MonoBehaviour
    {
        private static PlayerInput playerInput;

        public static PlayerInput GetPlayerInput()
        {
            if (playerInput == null)
            {
                playerInput = FindObjectOfType<PlayerInput>();
            }
            return playerInput;
        }

        public event Action<Vector2> PrimaryPressed;
        public event Action<Vector2> SecondaryPressed;

        public event Action<Vector2> DraggingStart;
        public event Action<Vector2> DraggingEnd;
        public event Action<Vector2> Dragging;

        public event Action<float> PullingIn;
        public event Action<float> PullingOut;


        #region EventRaising

        protected void RaisePrimaryPressed(Vector2 point)
        {
            PrimaryPressed?.Invoke(point);
        }

        protected void RaiseSecondaryPressed(Vector2 point)
        {
            SecondaryPressed?.Invoke(point);
        }

        protected void RaiseDraggingStart(Vector2 startPoint)
        {
            DraggingStart?.Invoke(startPoint);
        }

        protected void RaiseDraggingEnd(Vector2 endPoint)
        {
            DraggingEnd?.Invoke(endPoint);
        }

        protected void RaiseDragging(Vector2 currentPoint)
        {
            Dragging?.Invoke(currentPoint);
        }

        protected void RaisePullingIn(float pullingForce)
        {
            PullingIn?.Invoke(pullingForce);
        }

        protected void RaisePullingOut(float pullingForce)
        {
            PullingOut?.Invoke(pullingForce);
        }

        #endregion
    }
}