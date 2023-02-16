using RootCapsule.Core;
using RootCapsule.Core.Types;
using RootCapsule.ModelData.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace RootCapsule.Model.Fields
{
    // Developing: grows, dies, destroing, get seeds, harvesting, magic effects, save/load
    public class Plant : MonoBehaviour, IAlive, ISerializableObject<PlantData>
    {
        public const int LIFE_STAGE_COUNT = 3;

        [SerializeField] bool IsRandomCenter;
        [SerializeField] Vector2Int matrixSize = new Vector2Int(3, 4);
        [SerializeField] Vector2 partsPadding = new Vector2(0.5f, 1.5f);
        [SerializeField] float outlineMargin = 0.2f;

        [SerializeField] bool testObject = false;
        [SerializeField] string modelPath;

        PlantPart partPrefab;
        Arable arable;
        PlantPart[,] plantParts;

        SeedStat seedStat;
        PlantState plantState;
        Fertilizer fertilizer;

        public PlantType PlantType { get; private set; }
        public event Action Destruction;
        public bool Initialized { get; private set; }


        public void Initialize(PlantType plantType, SeedStat seedStat, PlantState plantState = new PlantState(), Vector3[,] partsPositions = null, Quaternion[,] partsRotations = null)
        {
            if (Initialized) return;

            if (arable.Fertilizer != null)
            {
                fertilizer = arable.Fertilizer;
                fertilizer.FertilizerOver += OnFertilizerOver;
            }

            this.PlantType = plantType;
            this.seedStat = seedStat;
            this.plantState = plantState;

            SetPlantParts(partsPositions, partsRotations);

            gameObject.SetActive(true);
            Initialized = true;
        }

        public void Deinitialize()
        {
            if (!Initialized) return;
            gameObject.SetActive(false);
            Initialized = false;
        }

        public Vector3[,] GetPlantPartsPositions()
        {
            var partsPositions = new Vector3[matrixSize.x, matrixSize.y];

            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    partsPositions[i, j] = plantParts[i, j].transform.position;
                }
            }

            return partsPositions;
        }

        void Awake()
        {
            partPrefab = PrefabHelper.GetFieldPrefab<PlantPart>();
            arable = GetArable();
            CreatePlantParts();
            if (!Initialized) gameObject.SetActive(false);
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

        void OnDrawGizmosSelected()
        {
            if (!IsRandomCenter) return;

            if (partPrefab == null)
                partPrefab = PrefabHelper.GetFieldPrefab<PlantPart>();

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

        void Start()
        {
            if (!Initialized && Application.isPlaying && !testObject)
            {
                throw new Exception(typeof(Plant).ToString() + " not initialized!");
            }
        }

        Arable GetArable()
        {
            var arable = gameObject.GetComponentInParent<Arable>();

            if (arable == null)
            {
                throw new Exception($"{typeof(Arable)} must be child of {typeof(Field)}!");
            }

            return arable;
        }

        void CreatePlantParts()
        {
            if (arable == null) throw new Exception(typeof(Plant) + " must be part of " + typeof(Arable));

            Bounds arableBounds = arable.GetComponentInChildren<Renderer>().bounds;
            Vector3 size = IsRandomCenter ? GetPlantPartSize(arableBounds) : Vector3.zero;
            plantParts = new PlantPart[matrixSize.x, matrixSize.y];

            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    Vector3 center = GetPlantPartCenter(i, j, arableBounds);
                    PlantPart plantPart = Instantiate(partPrefab, GetPlantPartRandomize(center, size), Quaternion.identity, transform);
                    plantParts[i, j] = plantPart;
                }
            }
        }

        void SetPlantParts(Vector3[,] partsPositions, Quaternion[,] partsRotations)
        {
            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    plantParts[i, j].SetState(PlantType.Id, plantState.LifeStage);
                    if (partsPositions != null) plantParts[i, j].transform.position = partsPositions[i, j];
                    if (partsRotations != null) plantParts[i, j].transform.rotation = partsRotations[i, j];
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
            var x = (arableBounds.size.x - outlineMargin * 2) / matrixSize.x - partsPadding.x;
            var z = (arableBounds.size.z - outlineMargin * 2) / matrixSize.y - partsPadding.y;
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
            if (plantState.LifePoints > PlantType.GrowthTime + PlantType.LifeTime)
            {
                Die();
            }
            else if (plantState.LifePoints >= PlantType.GrowthTime)
            {
                if (plantState.LifeStage != LifeStage.Adult && plantState.LifeStage != LifeStage.Refill)
                {
                    plantState.LifeStage = LifeStage.Adult;
                    GrowingUp();
                }
            }
            else
            {
                int stageIndex = (int)Math.Floor(LIFE_STAGE_COUNT * plantState.LifePoints / PlantType.GrowthTime);
                LifeStage stage = (LifeStage)stageIndex;
                if (stageIndex <= LIFE_STAGE_COUNT && plantState.LifeStage != stage)
                {
                    plantState.LifeStage = stage;
                    ChangeState();
                }
            }
        }

        void ChangeState()
        {
            foreach (var plantPart in plantParts)
            {
                plantPart.SetState(PlantType.Id, plantState.LifeStage);
            }
        }

        void GrowingUp()
        {
            foreach (var plantPart in plantParts)
            {
                plantPart.SetState(PlantType.Id, plantState.LifeStage);
            }
        }

        void Die()
        {
            Destruction?.Invoke();
        }

        #region Serialization
        public PlantData SerializeState()
        {
            VectorData[,] positionsArray = new VectorData[matrixSize.x, matrixSize.y];
            QuaternionData[,] rotationsArray = new QuaternionData[matrixSize.x, matrixSize.y];
            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    positionsArray[i, j] = plantParts[i, j].GetPositionData();
                    rotationsArray[i, j] = plantParts[i, j].GetRotationData();
                }
            }

            return new PlantData
            {
                PlantType = PlantType,
                PlantState = plantState,
                SeedStat = seedStat,
                PartsPositions = positionsArray,
                PartsRotations = rotationsArray
            };
        }

        public void DeserializeState(PlantData data)
        {
            Vector3[,] positionsArray = new Vector3[matrixSize.x, matrixSize.y];
            Quaternion[,] rotationsArray = new Quaternion[matrixSize.x, matrixSize.y];
            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    positionsArray[i, j] = new Vector3(data.PartsPositions[i, j].X, data.PartsPositions[i, j].Y, data.PartsPositions[i, j].Z);
                    rotationsArray[i, j] = new Quaternion(data.PartsRotations[i, j].X, data.PartsRotations[i, j].Y, data.PartsRotations[i, j].Z, data.PartsRotations[i, j].W);
                }
            }

            Initialize(data.PlantType, data.SeedStat, data.PlantState, positionsArray, rotationsArray);
        }
        #endregion
    }
}