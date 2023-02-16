using System.Threading.Tasks;

namespace RootCapsule.DataStorage.Repository
{
    public interface IRepository
    {
        Task SaveAsync<T>(string name, T obj) where T : struct;
        Task<T> LoadAsync<T>(string name) where T : struct;
    }
}
