using System;
using Metars.Models;
using SimpleMigrations;
using SQLite;

namespace Metars.Database
{
    public class SQLiteNetDatabaseProvider : IDatabaseProvider<SQLiteConnection>
    {
        private readonly SQLiteConnection connection;

        public SQLiteNetDatabaseProvider(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public SQLiteConnection BeginOperation()
        {
            return this.connection;
        }

        public void EndOperation()
        {
        }

        public long EnsurePrerequisitesCreatedAndGetCurrentVersion()
        {
            this.connection.CreateTable<SchemaVersion>();
            return this.GetCurrentVersion();
        }

        public long GetCurrentVersion()
        {
            // Return 0 if the table has no entries
            var latestOrNull = this.connection.Table<SchemaVersion>().OrderByDescending(x => x.Id).FirstOrDefault();
            return latestOrNull?.Version ?? 0;
        }

        public void UpdateVersion(long oldVersion, long newVersion, string newDescription)
        {
            this.connection.Insert(new SchemaVersion()
            {
                Version = newVersion,
                AppliedOn = DateTime.Now,
                Description = newDescription,
            });
        }
    }
}
