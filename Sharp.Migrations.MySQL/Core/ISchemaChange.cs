namespace Sharp.MySQL.Migrations.Core
{
    /// <summary>
    /// Interface base to class SchemaChanges
    /// </summary>
    public interface ISchemaChange
    {
        /// <summary>
        /// Version of changes
        /// </summary>
        int SchemaVersion { get; }

        /// <summary>
        /// Initializes the schema change
        /// </summary>
        /// <param name="factory">A connection to MySQLDB</param>
        void Initialize(ConnectionFactory factory);
        /// <summary>
        /// Sets if the change can runs
        /// </summary>
        /// <returns>True or false</returns>
        bool CanRun();
        /// <summary>
        /// Execute changes
        /// </summary>
        void Run();
    }
}
