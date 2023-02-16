using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace RootCapsule.DataStorage.Repository
{
    class BinaryRepository : IRepository
    {
        public async Task SaveAsync<T>(string fileName, T obj) where T : struct
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                await Task.Run(() => formatter.Serialize(stream, obj));
            }
        }

        public async Task<T> LoadAsync<T>(string fileName) where T : struct
        {
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return await Task.Run(() => (T)formatter.Deserialize(stream));
            }
        }
    }
}
