using System;
using System.Threading;
using SQLite;

namespace Metars.Services.Interfaces
{
    public interface ILocalDatabase
    {
        long CurrentDbVersion { get; }
        SQLiteAsyncConnection GetDbConnection(CancellationToken token);
        void InitializeDatabase();
        void ResetToEmptyDatabase();
        void DeleteExistingDatabase();
        void RecreateTable<TModel>();
    }
}
