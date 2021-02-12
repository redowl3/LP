using LaunchPad.Repository.LocalDbModels;
using SQLite;

namespace LaunchPad.Repository.Services
{
    public class ProductEntityServices : EntityService<Product>
    {
        public ProductEntityServices(SQLiteAsyncConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
