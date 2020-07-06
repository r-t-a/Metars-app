using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Metars.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using SQLite;
using Unity;

namespace Metars.Repositories
{
    public abstract class BaseRepository<TModel>
    {
        [Dependency]
        public ILocalDatabase LocalDatabase { get; set; }

        protected abstract Expression<Func<TModel, bool>> FindQuery(TModel item);

        public virtual async Task<List<T>> Query<T>(string sql, CancellationToken token, params object[] args) where T : new()
        {
            var conn = LocalDatabase.GetDbConnection(token);
            var results = new List<T>();

            if (conn != null)
            {
                try
                {
                    results = await conn.QueryAsync<T>(sql, args).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                }
            }

            token.ThrowIfCancellationRequested();

            return results;
        }

        public async Task RunInTransaction(Action<SQLiteConnection> transaction, CancellationToken token)
        {
            var conn = LocalDatabase.GetDbConnection(token);

            if (conn != null)
            {
                await conn.RunInTransactionAsync(transaction).ConfigureAwait(false);
            }

            token.ThrowIfCancellationRequested();
        }

        public virtual async Task<int> Execute(string sql, CancellationToken token, params object[] args)
        {
            var conn = LocalDatabase.GetDbConnection(token);
            var count = 0;

            if (conn != null)
            {
                try
                {
                    count = await conn.ExecuteAsync(sql, args).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                }
            }

            token.ThrowIfCancellationRequested();

            return count;
        }
    }
}
