using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RootCapsule.Core
{
    public static class PrefabHelper
    {
        const string PREFAB_FOLDER_NAME = "Prefab";
        const string FIELDS_FOLDER_NAME = "Fields";

        static Dictionary<Type, MonoBehaviour> prefabs = new Dictionary<Type, MonoBehaviour>();

        public static T GetFieldPrefab<T>() where T : MonoBehaviour
        {
            MonoBehaviour prefab;
            if (prefabs.TryGetValue(typeof(T), out prefab)) return (T)prefab;
            
            string path = Path.Combine(PREFAB_FOLDER_NAME, FIELDS_FOLDER_NAME, typeof(T).Name);
            prefab = Resources.Load<T>(path);
            if (prefab == null) throw new System.Exception($"Prefab {typeof(T).Name} not found in Resources. Path = {path}");

            prefabs.Add(typeof(T), prefab);
            return (T)prefab;
        }
    }
}
