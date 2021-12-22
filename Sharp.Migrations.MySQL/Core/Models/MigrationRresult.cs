namespace Sharp.MySQL.Migrations.Core.Models
{
    public class MigrationRresult
    {
        public int FirstSchemaVersion { get; set; }
        public int LastSchemaVersion { get; set; }
        public TableResult[] tables { get; set; }
    }
}