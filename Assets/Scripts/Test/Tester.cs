using RootCapsule.DataStorage.Iteractor;
using RootCapsule.DataStorage.Repository;
using RootCapsule.Model.Fields;
using RootCapsule.ModelData.Fields;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RootCapsule.Test
{
    public class Tester : MonoBehaviour
    {
        public void TestAction()
        {
            Serialize();
        }
        public void TestAction2()
        {
            Deserialize();
        }

        private void Serialize()
        {
            Debug.Log("Serialize Start!");

            Field[] fields = FindObjectsOfType<Field>();
            FieldData[] data = fields.Select(f => f.SerializeState()).ToArray();

            JsonRepository repository = new JsonRepository();
            var iteractor = new FieldIteractor(repository);
            iteractor.SaveData(data, out var exceptions);
            if (exceptions != null)
            {
                Debug.Log("There are some problems:");
                foreach (var exception in exceptions)
                {
                    Debug.Log(exception.Message);

                }
            }
            Debug.Log("Serialize over!");
        }

        private void Deserialize()
        {
            Debug.Log("Deserialize Start!");
            Field[] fields = FindObjectsOfType<Field>();

            JsonRepository repository = new JsonRepository();
            var iteractor = new FieldIteractor(repository);
            IEnumerable<FieldData> data = iteractor.LoadData(fields.Select(d => d.ID), out var exceptions);
            if (exceptions != null)
            {
                Debug.Log("There are some problems:");
                foreach (var exception in exceptions)
                {
                    Debug.Log(exception.Message);
                }
                return;
            }

            int i = 0;
            foreach (var item in data)
            {
                fields[i++].DeserializeState(item);
            }

            Debug.Log("Deserialize over!");
        }
    }
}