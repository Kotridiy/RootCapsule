using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RootCapsule.Core
{
    class ModelHelper
    {
        const string MODEL_FOLDER_NAME = "Model";
        const string PLANT_FOLDER_NAME = "Plants";
        const string MODEL_EXTENSION = ".fbx";

        static Dictionary<string, Mesh> models = new Dictionary<string, Mesh>();

        public static Mesh GetPlantModel(string fileName, string modelName)
        {
            Mesh model;
            string fullName = fileName + '\\' + modelName;
            if (models.TryGetValue(fullName, out model)) return model;

            string path = Path.Combine(MODEL_FOLDER_NAME, PLANT_FOLDER_NAME, fileName);
            Object[] resources = Resources.LoadAll(path);
            if (resources.Length == 0) throw new System.Exception($"No resources by path = {path}!");

            model = resources.Where(obj => obj.GetType() == typeof(Mesh)).Select(obj => obj as Mesh).Single(m => m.name == modelName);
            if (model == null) throw new System.Exception($"Model {fullName} not found in Resources. Path = {path}");

            models.Add(fullName, model);
            return model;
        }
    }
}
