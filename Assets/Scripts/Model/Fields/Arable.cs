using RootCapsule.Core;
using RootCapsule.Core.Types;
using RootCapsule.ModelData.Fields;
using System;
using UnityEngine;

namespace RootCapsule.Model.Fields
{
    // developing: row arable, weed attack, crossing
    [Serializable, RequireComponent(typeof(AudioSource))]
    public class Arable : MonoBehaviour, ISerializableObject<ArableData>
    {
        public Fertilizer Fertilizer { get; private set; }
        public IAlive AliveOnArable { get; private set; }

        public Vector2Int IndexPosition { get; private set; }

        public AudioClip PlantSound;

        private Plant plantBlank;
        private DeadPlant deadPlantBlank;
        private Field field;

        private AudioSource audioSource;

        private bool initialized = false;


        public void Initialize(int iPos, int jPos)
        {
            if (initialized) return;

            IndexPosition = new Vector2Int(iPos, jPos);
            field = GetField();
            initialized = true;
        }

        public void PlantSeed(Seed seed)
        {
            audioSource.clip = PlantSound;
            audioSource.Play();

            ActivatePlant(plantBlank);
            plantBlank.Initialize(seed.PlantType, seed.SeedStat);
        }

        void ActivatePlant(IAlive alive)
        {
            if (AliveOnArable != null) throw new InvalidOperationException($"{typeof(Arable)} can't store more that one plant!");

            AliveOnArable = alive;
            AliveOnArable.Destruction += OnDestruction;
        }

        void DeactivatePlant()
        {
            if (AliveOnArable == null) throw new InvalidOperationException($"{typeof(Arable)} can't deactivate nothing!");

            AliveOnArable.Destruction -= OnDestruction;
            AliveOnArable = null;
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

        void Awake()
        {
            var plantPrefab = PrefabHelper.GetFieldPrefab<Plant>();
            plantBlank = Instantiate(plantPrefab, transform);

            var deadPlantPrefab = PrefabHelper.GetFieldPrefab<DeadPlant>();
            deadPlantBlank = Instantiate(deadPlantPrefab, transform);

            audioSource = GetComponent<AudioSource>();
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
            AliveOnArable.Deinitialize();
            Plant plant = AliveOnArable as Plant;
            if (AliveOnArable is Plant)
            {
                DeactivatePlant();

                deadPlantBlank.Initialize(plant.PlantType, plant.GetPlantPartsPositions());

                ActivatePlant(deadPlantBlank);
            }
            else if (AliveOnArable is DeadPlant)
            {
                DeactivatePlant();
            }
        }

        void OnTick()
        {
            // TODO
        }

        #region Serialization
        public ArableData SerializeState()
        {
            var data = new ArableData
            {
                PositionX = IndexPosition.x,
                PositionY = IndexPosition.y
            };

            if (Fertilizer != null)
                data.Fertilizer = Fertilizer.SerializeState();

            if (AliveOnArable is Plant)
                data.Plant = (AliveOnArable as Plant).SerializeState();

            else if (AliveOnArable is DeadPlant)
                data.DeadPlant = (AliveOnArable as DeadPlant).SerializeState();
            
            return data;
        }

        public void DeserializeState(ArableData data)
        {
            Initialize(data.PositionX, data.PositionY);
            Fertilizer = null;
            if (AliveOnArable != null)
            {
                AliveOnArable.Deinitialize();
                DeactivatePlant();
            }

            if (data.Fertilizer != null)
            {
                Fertilizer = new Fertilizer(data.Fertilizer.Value);
            }

            if (data.Plant != null)
            {
                ActivatePlant(plantBlank);
                plantBlank.DeserializeState(data.Plant.Value);
            }

            else if (data.DeadPlant != null)
            {
                ActivatePlant(deadPlantBlank);
                deadPlantBlank.DeserializeState(data.DeadPlant.Value);
            }
        }
        #endregion
    }
}
