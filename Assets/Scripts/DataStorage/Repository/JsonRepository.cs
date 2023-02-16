using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RootCapsule.DataStorage.Repository
{
    class JsonRepository : IRepository
    {
        const string FILE_EXTENSION = ".json";

        string repositoryPath;

        public JsonRepository(string path = "")
        {
            repositoryPath = path;
        }

        public async Task SaveAsync<T>(string fileName, T obj) where T : struct
        {
            string json = JsonConvert.SerializeObject(obj);
            //string json = JsonUtility.ToJson(obj);
            string folderPath = Path.Combine(Application.streamingAssetsPath, repositoryPath);
            string fullPath = Path.Combine(folderPath, fileName + FILE_EXTENSION);

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write); 
            using (StreamWriter sw = new StreamWriter(fileStream))
            {
                await sw.WriteAsync(json);
            }
        }

        public async Task<T> LoadAsync<T>(string fileName) where T : struct
        {
            string folderPath = Path.Combine(Application.streamingAssetsPath, repositoryPath);
            string fullPath = Path.Combine(folderPath, fileName + FILE_EXTENSION);
            string json = string.Empty;

            using (StreamReader sr = new StreamReader(fullPath))
            {
                json = await sr.ReadToEndAsync();
            }

            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
    }
}