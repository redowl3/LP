using Akavache;
using LaunchPad.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseServices))]
namespace LaunchPad.Mobile.Services
{
    public class DatabaseServices:IDatabaseServices
    {
        private static DatabaseServices _instance;
        public static DatabaseServices Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseServices();
                return _instance;
            }
        }
        internal IBlobCache DbInstance => BlobCache.UserAccount;

        public DatabaseServices()
        {
            try
            {
                BlobCache.ApplicationName = "BarelandsFarm";
                BlobCache.EnsureInitialized();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task<bool> Clear()
        {
            try
            {
                await DbInstance.InvalidateAll();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete<T>(string key)
        {
            try
            {
                await DbInstance.InvalidateObject<T>(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAllOfType<T>()
        {
            try
            {
                await DbInstance.InvalidateAllObjects<T>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<T> Get<T>(string key)
        {
            var entity = Activator.CreateInstance<T>();
            try
            {
                entity = await DbInstance.GetObject<T>(key);
            }
            catch (Exception ex)
            {

            }
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllObjectOfType<T>()
        {
            var entity = Activator.CreateInstance<IEnumerable<T>>();
            try
            {
                entity = await DbInstance.GetAllObjects<T>();
            }
            catch (Exception)
            {

            }
            return entity.ToList();
        }

        public async Task<bool> InsertData<T>(string key, T t)
        {
            try
            {
                await DbInstance.InsertObject(key, t);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
