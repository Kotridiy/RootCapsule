using RootCapsule.Core;
using RootCapsule.Core.Types;
using RootCapsule.ModelData.Fields;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RootCapsule.Model.Fields
{
    // developing: containg model and state, change state to change model
    [RequireComponent(typeof(MeshFilter))]
    public class PlantPart : MonoBehaviour
    {
        MeshFilter meshFilter;


        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        public void SetState(string typeName, LifeStage stage)
        {
            StringBuilder modelNameBuilder = new StringBuilder("tree_01_");

            switch (stage)
            {
                case LifeStage.New:
                    modelNameBuilder.Append("start");
                    break;
                case LifeStage.Child:
                    modelNameBuilder.Append("mid");
                    break;
                case LifeStage.Teen:
                    return;
                case LifeStage.Adult:
                    modelNameBuilder.Append("end");
                    break;
                case LifeStage.Refill:
                    return;
            }
            string modelName = modelNameBuilder.ToString();

            meshFilter.mesh = ModelHelper.GetPlantModel(typeName, modelName);
            RotateYAsix();
        }

        public void SetDeadState(string typeName)
        {
            meshFilter.mesh = ModelHelper.GetPlantModel(typeName, "tree_02_mid");
            RotateYAsix();
        }

        public VectorData GetPositionData()
        {
            return new VectorData
            {
                X = transform.position.x,
                Y = transform.position.y,
                Z = transform.position.z
            };
        }

        public QuaternionData GetRotationData()
        {
            return new QuaternionData
            {
                X = transform.rotation.x,
                Y = transform.rotation.y,
                Z = transform.rotation.z,
                W = transform.rotation.w
            };
        }

        void RotateYAsix()
        {
            transform.Rotate(Vector3.up, 90 * Random.Range(0, 4));
        }
    }
}