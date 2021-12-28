namespace Sharp.MySQL.Migrations.Core.Models
{
    /// <summary>
    /// Result of migration
    /// </summary>
    public class MigrationResult
    {
        /// <summary>
        /// First version
        /// </summary>
        public int FirstSchemaVersion { get; set; }
        /// <summary>
        /// Last version of changes
        /// </summary>
        public int LastSchemaVersion { get; set; }
        /// <summary>
        /// Results of tables migration
        /// </summary>
        public TableResult[] tables { get; set; }
    }
}