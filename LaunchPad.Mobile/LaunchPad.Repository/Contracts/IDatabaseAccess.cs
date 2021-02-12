using SQLite;

namespace LaunchPad.Repository.Contracts
{
    public interface IDatabaseAccess
    {
        SQLiteAsyncConnection GetConnection();
    }
}
