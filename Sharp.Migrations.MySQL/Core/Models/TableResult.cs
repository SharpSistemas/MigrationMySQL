namespace Sharp.MySQL.Migrations.Core.Models
{
    /// <summary>
    /// Result of migration
    /// </summary>
    public class TableResult
    {
        /// <summary>
        /// Table name
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Indicates if the table was created
        /// </summary>
        public bool WasCreated { get; set; }
        /// <summary>
        /// Indicates if the table was modified
        /// </summary>
        public bool WasModified { get; set; }
        /// <summary>
        /// Indicates how many columns was added
        /// </summary>
        public int ColumnsAdded { get; set; }
    }
}
