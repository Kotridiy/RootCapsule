using RootCapsule.Core;
using System;
using UnityEngine;

namespace RootCapsule.Model.Fields
{
    // developing: row arable, weed attack, crossing
    public class Arable : MonoBehaviour, ITimeDependent
    {
        public Vector2Int IndexPosition { get; private set; }

        public Fertilizer Fertilizer { get; private set; }
        public IAlive AliveOnArable { get; set; }

        Field field;

        bool initialized = false;

        public void Initialize(int iPos, int jPos)
        {
            if (initialized) return;

            IndexPosition = new Vector2Int(iPos, jPos);
            field = GetField();
            initialized = true;
        }

        Field GetField()
        {
            var field = gameObject.GetComponentInParent<Field>();

            if (field == null)
            {
                throw new Exception($"{typeof(Arable)} must be child of {typeof(Field)}!");
            }

            return field;
        }

        private void Start()
        {
            if (!initialized && Application.isPlaying)
            {
                throw new Exception(typeof(Arable).ToString() + " not initialized!");
            }

            // TEST CODE
            Fertilizer = new Fertilizer(5);
            Fertilizer.FertilizerOver += OnFertilizerOver;
        }

        private void OnEnable()
        {
            if (Fertilizer != null) Fertilizer.FertilizerOver += OnFertilizerOver;
        }

        private void OnDisable()
        {
            if (Fertilizer != null) Fertilizer.FertilizerOver -= OnFertilizerOver;
        }

        void OnFertilizerOver()
        {
            Fertilizer = null;
        }

        void ITimeDependent.OnTick()
        {
            // TODO
        }
    }
}
