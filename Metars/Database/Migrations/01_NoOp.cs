using System;
using SimpleMigrations;

namespace Metars.Database.Migrations
{
    [Migration(01, "NoOp")]
    public class CreateDatabase : SQLiteNetMigration
    {
        public override void Up()
        {
            //No op
        }

        ///<inheritdoc/>
        public override void Down()
        {
            //No op
        }
    }
}
