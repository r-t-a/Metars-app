using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Metars.Models;
using Metars.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using SimpleMigrations;
using SQLite;

namespace Metars.Database
{
    public class LocalDatabase : ILocalDatabase
    {
        public long CurrentDbVersion => GetCurrentVersion();

        private static string DatabaseFileName => "Metars.db";

        public void InitializeDatabase()
        {
            PrepareUserDatabase();
        }

        public void DeleteExistingDatabase()
        {
            DeleteDatabases();
        }

        public SQLiteAsyncConnection GetDbConnection(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var databasePath = Path.Combine(GetDatabaseFolder(), DatabaseFileName);
            return File.Exists(databasePath) ? new SQLiteAsyncConnection(databasePath) : null;
        }

        public void RecreateTable<TModel>()
        {
            try
            {
                var databasePath = GetActiveDatabasePath();

                using (var conn = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create))
                {
                    conn.CreateTable<TModel>();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void ResetToEmptyDatabase()
        {
            InitializeDatabase();
        }

        protected void CreateInitialDatabase(string storagePath)
        {
            using (var conn = new SQLiteConnection(storagePath))
            {
                CreateTables(conn);
                var migrator = GetMigrator(conn);
                migrator.Baseline(migrator.LatestMigration.Version);
            }
        }

        protected virtual string GetActiveDatabasePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseFileName);
        }

        protected virtual string GetDatabaseFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        protected SimpleMigrator<SQLiteConnection, SQLiteNetMigration> GetMigrator(SQLiteConnection connection)
        {
            var migrationDbProvider = new SQLiteNetDatabaseProvider(connection);
            var migrator = new SimpleMigrator<SQLiteConnection, SQLiteNetMigration>(
                typeof(LocalDatabase).GetTypeInfo().Assembly,
                migrationDbProvider);
            migrator.Load();
            return migrator;
        }

        protected void CreateTables(SQLiteConnection conn)
        {
            conn.CreateTable<Airport>();
            conn.CreateTable<Station>();
        }

        private bool MigrateToLatest(string databasePath)
        {
            bool versionChanged;
            long previousVersion;
            long newVersion;

            using (var conn = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create))
            {
                var migrator = GetMigrator(conn);
                previousVersion = migrator.CurrentMigration.Version;
                migrator.MigrateToLatest();
                newVersion = migrator.CurrentMigration.Version;
                versionChanged = previousVersion != newVersion;
            }

            if (versionChanged)
            {
                var versionsToTrack = new Dictionary<string, string>()
                {
                    {"Previous Version", previousVersion.ToString() },
                    {"Current Version", newVersion.ToString() }
                };
            }

            return versionChanged;
        }

        private long GetCurrentVersion()
        {
            var databasePath = GetActiveDatabasePath();
            using (var conn = new SQLiteConnection(databasePath))
            {
                var migrator = GetMigrator(conn);
                return migrator.CurrentMigration.Version;
            }
        }

        private void DeleteDatabases()
        {
            var succeeded = false;

            for (var retries = 0; retries < 3 && !succeeded; retries++)
            {
                try
                {
                    var databasePath = Path.Combine(GetDatabaseFolder(), DatabaseFileName);
                    var databaseExists = File.Exists(databasePath);

                    if (databaseExists)
                    {
                        using (var conn = new SQLiteConnection(databasePath))
                        {
                        }
                        succeeded = true;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }

            var dbPath = GetActiveDatabasePath();
            if (string.IsNullOrEmpty(dbPath)) return;
            try
            {
                File.Delete(dbPath);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void PrepareUserDatabase()
        {
            var succeeded = false;
            for (var retries = 0; retries < 3 && !succeeded; retries++)
            {
                try
                {
                    var databasePath = string.Empty;
                    var databaseExists = false;

                    if (!string.IsNullOrEmpty(DatabaseFileName))
                    {
                        databasePath = Path.Combine(GetDatabaseFolder(), DatabaseFileName);
                        databaseExists = File.Exists(databasePath);
                    }

                    if (databaseExists)
                    {
                        MigrateToLatest(databasePath);
                    }
                    else if (!string.IsNullOrEmpty(databasePath))
                    {
                        CreateInitialDatabase(databasePath);
                    }

                    succeeded = true;
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                    succeeded = false;
                }
            }

            if (!succeeded)
            {
                Crashes.TrackError(new Exception("Migration failed after retries"));
            }
        }
    }
}
