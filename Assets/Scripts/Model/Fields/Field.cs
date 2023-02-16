using RootCapsule.Core;
using RootCapsule.Core.Types;
using RootCapsule.ModelData.Fields;
using System;
using System.Linq;
using UnityEngine;

namespace RootCapsule.Model.Fields
{
    // developing: row field
    public class Field : MonoBehaviour, ISerializableObject<FieldData>
    {
        public string ID { get; private set; }

        [SerializeField] Vector2Int matrixSize = new Vector2Int(3, 5);
        [SerializeField] Vector2 arablePadding = new Vector2(0.5f, 1.5f);

        Arable[] arables;
        Arable arablePrefab;
        Vector3 size;

        void Awake()
        {
            ID = ID ?? gameObject.name;
            arablePrefab = PrefabHelper.GetFieldPrefab<Arable>();
            size = arablePrefab.GetComponentInChildren<Renderer>().bounds.size;
            CreateArables();
        }

        private void CreateArables(ArableData[] arableData = null)
        {
            arables = new Arable[matrixSize.x * matrixSize.y];
            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    Arable currentArable = Instantiate(arablePrefab, GetPosition(i, j), Quaternion.identity, transform);
                    int arableIndex = matrixSize.y * i + j;
                    if (arableData != null)
                    {
                        currentArable.DeserializeState(arableData[arableIndex]);
                    }
                    else
                    {
                        currentArable.Initialize(i, j);
                    }
                    arables[arableIndex] = currentArable;
                }
            }
        }

        void OnDrawGizmos()
        {
            if (arablePrefab == null)
                arablePrefab = PrefabHelper.GetFieldPrefab<Arable>();

            if (size == Vector3.zero)
                size = arablePrefab.GetComponentInChildren<Renderer>().bounds.size;

            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    Gizmos.DrawWireCube(GetPosition(i, j), size);
                }
            }
        }

        Vector3 GetPosition(int i, int j)
        {
            return transform.position + new Vector3((size.x + arablePadding.x) * i, 0, (size.z + arablePadding.y) * j);
        }

        #region Serialization
        public FieldData SerializeState()
        {
            return new FieldData
            {
                ID = ID,
                Arables = arables.Select(a => a.SerializeState()).ToArray()
            };
        }

        public void DeserializeState(FieldData data)
        {
            ID = data.ID;
            if (arables != null)
            {
                DeserializeArables(data.Arables);
            }
            else
            {
                CreateArables(data.Arables);
            }
        }

        private void DeserializeArables(ArableData[] arableData)
        {
            if (arables == null) throw new InvalidOperationException("Arable collection must be created!");

            for (int i = 0; i < arables.Length; i++)
            {
                arables[i].DeserializeState(arableData[i]);
            }
        }
        #endregion
    }
}