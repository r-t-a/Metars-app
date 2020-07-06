using System;
using SimpleMigrations;
using SQLite;

namespace Metars.Database
{
    public abstract class SQLiteNetMigration : IMigration<SQLiteConnection>
    {
        protected bool UseTransaction { get; set; }
        protected SQLiteConnection Connection { get; set; }
        protected IMigrationLogger Logger { get; set; }

        public abstract void Up();
        public abstract void Down();

        public void Execute(string sql)
        {
            this.Logger.LogSql(sql);
            this.Connection.Execute(sql);
        }

        void IMigration<SQLiteConnection>.RunMigration(MigrationRunData<SQLiteConnection> data)
        {
            this.Connection = data.Connection;
            this.Logger = data.Logger;

            if (this.UseTransaction)
            {
                try
                {
                    this.Connection.BeginTransaction();

                    if (data.Direction == MigrationDirection.Up)
                        this.Up();
                    else
                        this.Down();

                    this.Connection.Commit();
                }
                catch
                {
                    this.Connection.Rollback();
                    throw;
                }
            }
            else
            {
                if (data.Direction == MigrationDirection.Up)
                    this.Up();
                else
                    this.Down();
            }
        }
    }
}
