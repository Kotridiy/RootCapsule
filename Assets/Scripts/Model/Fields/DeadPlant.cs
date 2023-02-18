using RootCapsule.Core;
using RootCapsule.Core.Types;
using RootCapsule.ModelData.Fields;
using System;
using UnityEngine;

namespace RootCapsule.Model.Fields
{
    // developing: destroy
    public class DeadPlant : MonoBehaviour, IAlive, ISerializableObject<DeadPlantData>
    {
        [SerializeField] Vector2Int matrixSize = new Vector2Int(3, 4);

        public event Action Destruction;

        private Arable arable;
        private PlantType plantType;

        private PlantPart partPrefab;
        private PlantPart[,] plantParts;
        
        public bool Initialized { get; set; }


        public void Initialize(PlantType plantType, Vector3[,] partsPositions, Quaternion[,] partsRotations = null)
        {
            if (Initialized) return;

            this.plantType = plantType;

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

        public void Weed()
        {
            Destroy();
        }

        void Awake()
        {
            partPrefab = PrefabHelper.GetFieldPrefab<PlantPart>();
            arable = GetArable();
            CreatePlantParts();
            if (!Initialized) gameObject.SetActive(false);
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

        void Destroy()
        {
            gameObject.SetActive(false);
            Destruction?.Invoke();
        }

        void CreatePlantParts()
        {
            if (arable == null) throw new Exception(typeof(DeadPlant) + " must be part of " + typeof(Arable));

            plantParts = new PlantPart[matrixSize.x, matrixSize.y];

            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    PlantPart plantPart = Instantiate(partPrefab, transform);

                    plantParts[i, j] = plantPart;
                }
            }
        }

        void SetPlantParts(Vector3[,] partsPositions, Quaternion[,] partsRotations)
        {
            if (matrixSize.x != partsPositions.GetLength(0) || matrixSize.y != partsPositions.GetLength(1))
                throw new Exception(typeof(DeadPlant) + " has wrong matrix size!");

            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    plantParts[i, j].SetDeadState(plantType.Id);
                    plantParts[i, j].transform.position = partsPositions[i, j];
                    if (partsRotations != null) plantParts[i, j].transform.rotation = partsRotations[i, j];
                }
            }
        }

        #region Serialization
        public DeadPlantData SerializeState()
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

            return new DeadPlantData
            {
                PlantType = plantType,
                PartsPositions = positionsArray,
                PartsRotations = rotationsArray
            };
        }

        public void DeserializeState(DeadPlantData data)
        {
            if (matrixSize.x != data.PartsPositions.GetLength(0) || matrixSize.y != data.PartsPositions.GetLength(1))
                throw new Exception(typeof(DeadPlant) + " has wrong matrix size!");

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

            Initialize(data.PlantType, positionsArray, rotationsArray);
        }
        #endregion
    }
}