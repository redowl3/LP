using LaunchPad.Repository.Contracts;
using LaunchPad.Repository.Extensions;
using LaunchPad.Repository.LocalDbModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace LaunchPad.Repository
{
    public class EntityService<T> : IEntityService<T> where T : BaseEntity, new()
    {
        private SQLiteAsyncConnection _dbConnection;
        static bool initialized = false;
        public EntityService(SQLiteAsyncConnection dbConnection)
        {
            _dbConnection = dbConnection;
            InitializeAsync().SafeFireAndForget(false);
        }
        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!_dbConnection.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
                {
                    await _dbConnection.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public AsyncTableQuery<T> AsQueryable()
        {
            return _dbConnection.Table<T>();
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate = null)
        {
            var query = _dbConnection.Table<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync();
        }

        public async Task<int> Delete(T entity)
        {
            return await _dbConnection.DeleteAsync(entity);
        }

        public async Task<List<T>> Get()
        {
            return await _dbConnection.Table<T>().ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return await _dbConnection.FindAsync<T>(predicate);
        }

        public async Task<T> Get(int id)
        {
            return await _dbConnection.FindAsync<T>(id);
        }

        public async Task<ObservableCollection<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = _dbConnection.Table<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = query.OrderBy<TValue>(orderBy);
            }

            var collection = new ObservableCollection<T>();
            var items = await query.ToListAsync();
            foreach (var item in items)
            {
                collection.Add(item);
            }

            return collection;
        }

        public async Task<int> Insert(T entity)
        {
            return await _dbConnection.InsertAsync(entity);
        }

        public async Task<int> Update(T entity)
        {
            return await _dbConnection.UpdateAsync(entity);
        }
    }
}
