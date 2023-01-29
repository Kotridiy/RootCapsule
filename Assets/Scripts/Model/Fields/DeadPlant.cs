using System;
using UnityEngine;

namespace RootCapsule.Model.Fields
{
    // developing: store model of dead plant, destroy
    public class DeadPlant : MonoBehaviour, IAlive
    {
        public event Action Destruction;

        public void Weed()
        {
            Destroy();
        }

        void Destroy()
        {
            Destruction?.Invoke();
            Destroy(gameObject);
        }
    }
}