using RootCapsule.Core;
using System;
using UnityEngine;

namespace RootCapsule.Model.Fields
{
    // developing: row arable, weed attack, crossing
    public class Arable : MonoBehaviour
    {
        [SerializeField] private Plant plantPrefab;

        public Vector2Int IndexPosition { get; private set; }

        public Fertilizer Fertilizer { get; private set; }
        public IAlive AliveOnArable { get; private set; }

        Field field;

        bool initialized = false;

        public void Initialize(int iPos, int jPos)
        {
            if (initialized) return;

            IndexPosition = new Vector2Int(iPos, jPos);
            field = GetField();
            initialized = true;
        }

        public void PlantSeed(Seed seed)
        {
            if (AliveOnArable != null) throw new InvalidOperationException($"{typeof(Arable)} can't store more that one plant!");

            var newPlant = Instantiate(plantPrefab, transform);
            newPlant.Initialize(this, seed.PlantType, seed.SeedStat, Fertilizer);
            AliveOnArable = newPlant;
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

        void Start()
        {
            if (!initialized && Application.isPlaying)
            {
                throw new Exception(typeof(Arable).ToString() + " not initialized!");
            }

            // TEST CODE
            Fertilizer = new Fertilizer(5);
            Fertilizer.FertilizerOver += OnFertilizerOver;
        }

        void OnEnable()
        {
            if (Fertilizer != null) Fertilizer.FertilizerOver += OnFertilizerOver;
            if (AliveOnArable != null) AliveOnArable.Destruction += OnDestruction;
        }

        void OnDisable()
        {
            if (Fertilizer != null) Fertilizer.FertilizerOver -= OnFertilizerOver;
            if (AliveOnArable != null) AliveOnArable.Destruction += OnDestruction;
        }

        void OnFertilizerOver()
        {
            Fertilizer = null;
        }

        void OnDestruction()
        {
            AliveOnArable = null;
        }

        void OnTick()
        {
            // TODO
        }
    }
}
