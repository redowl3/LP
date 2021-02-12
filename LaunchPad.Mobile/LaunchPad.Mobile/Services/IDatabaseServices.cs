using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaunchPad.Mobile.Services
{
    public interface IDatabaseServices
    {
        Task<bool> InsertData<T>(string key, T t);
        Task<T> Get<T>(string key);
        Task<IEnumerable<T>> GetAllObjectOfType<T>();
        Task<bool> Delete<T>(string key);
        Task<bool> DeleteAllOfType<T>();
        Task<bool> Clear();
    }
}
