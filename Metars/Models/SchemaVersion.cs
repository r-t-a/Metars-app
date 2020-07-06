using System;
using SQLite;

namespace Metars.Models
{
    public class SchemaVersion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public long Version { get; set; }
        public DateTime AppliedOn { get; set; }
        public string Description { get; set; }
    }
}
