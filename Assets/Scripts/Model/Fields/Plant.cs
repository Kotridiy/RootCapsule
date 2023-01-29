using RootCapsule.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityRandom = UnityEngine.Random;

namespace RootCapsule.Model.Fields
{
    // Developing: grows, dies, destroing, get seeds, harvesting, magic effects, save/load
    public class Plant : MonoBehaviour, IAlive
    {
        public const int LIFE_STAGE_COUNT = 3;

        [SerializeField] PlantPart partPrefab;
        [SerializeField] bool IsRandomCenter;
        [SerializeField] Vector2Int matrixSize = new Vector2Int(3, 4);
        [SerializeField] Vector2 arablePadding = new Vector2(0.5f, 1.5f);
        [SerializeField] float outlineMargin = 0.2f;

        [SerializeField] bool testObject = false;
        [SerializeField] string modelPath;

        Arable arable;
        List<PlantPart> plantParts;
        PlantType plantType;
        SeedStat seedStat;
        PlantState plantState;
        Fertilizer fertilizer;
        bool initialized = false;

        // TEST CODE
        UnityObject[] models;

        public event Action Destruction;


        public void Initialize(Arable arable, PlantType plantType, SeedStat seedStat, Fertilizer fertilizer, PlantState state = new PlantState())
        {
            if (initialized) return;

            this.arable = arable ?? throw new ArgumentNullException(nameof(arable));
            this.plantType = plantType;
            this.seedStat = seedStat;
            if (fertilizer != null)
            {
                this.fertilizer = fertilizer;
                fertilizer.FertilizerOver += OnFertilizerOver;
            }
            plantState = state;

            // TEST CODE
            models = AssetDatabase.LoadAllAssetRepresentationsAtPath(modelPath).Where(obj => obj.GetType() == typeof(Mesh)).Select(obj => obj as Mesh).ToArray();
            CreatePlantParts();

            initialized = true;
        }

        void OnEnable()
        {
            if (fertilizer != null) fertilizer.FertilizerOver += OnFertilizerOver;
            WorldTime.GetWorldTime().Tick += OnTick;
        }

        void OnDisable()
        {
            if (fertilizer != null) fertilizer.FertilizerOver -= OnFertilizerOver;
            WorldTime.GetWorldTime().Tick -= OnTick;
        }

        void OnDrawGizmos()
        {
            if (!IsRandomCenter) return;

            Gizmos.color = Color.green;

            if (arable != null)
            {
                Bounds arableBounds = arable.GetComponentInChildren<Renderer>().bounds;
                Vector3 size = GetPlantPartSize(arableBounds);

                for (int i = 0; i < matrixSize.x; i++)
                {
                    for (int j = 0; j < matrixSize.y; j++)
                    {
                        Gizmos.DrawWireCube(GetPlantPartCenter(i, j, arableBounds), size);
                    }
                }
            }
        }

        void CreatePlantParts()
        {
            if (arable == null) throw new Exception(typeof(Plant) + " must be part of " + typeof(Arable));

            Bounds arableBounds = arable.GetComponentInChildren<Renderer>().bounds;
            Vector3 size = IsRandomCenter ? GetPlantPartSize(arableBounds) : Vector3.zero;
            plantParts = new List<PlantPart>();

            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    Vector3 center = GetPlantPartCenter(i, j, arableBounds);
                    PlantPart plantPart = Instantiate(partPrefab, GetPlantPartRandomize(center, size), Quaternion.identity, transform);
                    plantPart.GetComponent<MeshFilter>().mesh = (models as Mesh[]).Single(m => m.name == "tree_01_start");
                    plantParts.Add(plantPart);
                }
            }
        }

        Vector3 GetPlantPartCenter(int i, int j, Bounds arableBounds)
        {
            var x = arableBounds.min.x + (arableBounds.size.x - outlineMargin * 2) / matrixSize.x * (i + 0.5f) + outlineMargin;
            var z = arableBounds.min.z + (arableBounds.size.z - outlineMargin * 2) / matrixSize.y * (j + 0.5f) + outlineMargin;
            var y = transform.position.y;
            return new Vector3(x, y, z);
        }

        Vector3 GetPlantPartSize(Bounds arableBounds)
        {
            Bounds plantBounds = partPrefab.GetComponent<Renderer>().bounds;
            var x = (arableBounds.size.x - outlineMargin * 2) / matrixSize.x - arablePadding.x;
            var z = (arableBounds.size.z - outlineMargin * 2) / matrixSize.y - arablePadding.y;
            var y = plantBounds.size.y;
            return new Vector3(x, y, z);
        }

        Vector3 GetPlantPartRandomize(Vector3 center, Vector3 size)
        {
            Vector3 randomCenter = new Vector3();
            randomCenter.x = center.x + UnityRandom.Range(-size.x / 2, size.x / 2);
            randomCenter.z = center.z + UnityRandom.Range(-size.z / 2, size.z / 2);
            randomCenter.y = center.y;
            return randomCenter;
        }

        void OnFertilizerOver()
        {
            fertilizer = null;
        }

        void OnTick()
        {
            plantState.LifePoints += fertilizer != null ? fertilizer.GrowthModifier : 1;
            fertilizer?.Use();

            CheckLifeProgress();
            Debug.Log("Plant tick");

            // TODO
        }

        void CheckLifeProgress()
        {
            if (plantState.LifePoints > plantType.GrowthTime + plantType.LifeTime)
            {
                Die();
            }
            else if (plantState.LifePoints >= plantType.GrowthTime)
            {
                if (plantState.LifeStage != LifeStage.Adult && plantState.LifeStage != LifeStage.Refill)
                {
                    plantState.LifeStage = LifeStage.Adult;
                    GrowingUp();
                }
            }
            else
            {
                int stageIndex = (int)Math.Floor(LIFE_STAGE_COUNT * plantState.LifePoints / plantType.GrowthTime);
                LifeStage stage = (LifeStage)stageIndex;
                if (stageIndex <= LIFE_STAGE_COUNT && plantState.LifeStage != stage)
                {
                    plantState.LifeStage = stage;
                    ChangeState(stageIndex);
                }
            }
        }

        void ChangeState(int stage)
        {
            foreach (var plantPart in plantParts)
            {
                plantPart.GetComponent<MeshFilter>().mesh = (models as Mesh[]).Single(m => m.name == "tree_01_mid");
            }
            Debug.Log("ChangeState: " + (LifeStage)stage);
        }

        void GrowingUp()
        {
            foreach (var plantPart in plantParts)
            {
                plantPart.GetComponent<MeshFilter>().mesh = (models as Mesh[]).Single(m => m.name == "tree_01_end");
            }
            Debug.Log("GrowingUp");
        }

        void Die()
        {
            Debug.Log($"Plant {arable.IndexPosition.x}, {arable.IndexPosition.x} die!");
            Destruction?.Invoke();
            Destroy(gameObject);
        }

        void Start()
        {
            if (!initialized && Application.isPlaying && !testObject)
            {
                throw new Exception(typeof(Plant).ToString() + " not initialized!");
            }
        }
    }
}
