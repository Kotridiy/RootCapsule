using UnityEngine;

namespace RootCapsule.Model.Fields
{
    // developing: row field
    public class Field : MonoBehaviour
    {
        [SerializeField] Arable arable;
        [SerializeField] Vector2Int matrixSize = new Vector2Int(3, 5);
        [SerializeField] Vector2 arablePadding = new Vector2(0.5f, 1.5f);

        Vector3 size;

        private void Awake()
        {
            size = arable.GetComponentInChildren<Renderer>().bounds.size;
            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    Arable currentArable = Instantiate(arable, GetPosition(i, j), Quaternion.identity, transform);
                    currentArable.Initialize(i, j);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (size == Vector3.zero)
                size = arable.GetComponentInChildren<Renderer>().bounds.size;

            for (int i = 0; i < matrixSize.x; i++)
            {
                for (int j = 0; j < matrixSize.y; j++)
                {
                    Gizmos.DrawWireCube(GetPosition(i, j), size);
                }
            }
        }

        private Vector3 GetPosition(int i, int j)
        {
            return transform.position + new Vector3((size.x + arablePadding.x) * i, 0, (size.z + arablePadding.y) * j);
        }
    }
}