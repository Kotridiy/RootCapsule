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

        public void ChangeState(LifeStage stage, Mesh[] models)
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

            meshFilter.mesh = models.Single(m => m.name == modelName);
            RotateYAsix();
        }

        public void Die(Mesh[] models)
        {
            meshFilter.mesh = models.Single(m => m.name == "tree_02_mid");
        }

        void RotateYAsix()
        {
            //transform.Rotate(Vector3.up, Random.Range(0, 360));
            transform.Rotate(Vector3.up, 90 * Random.Range(0, 4));
        }
    }
}