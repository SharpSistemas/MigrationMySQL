
using Sharp.MySQL.Migrations.Core.Models;

namespace Sharp.MySQL.Migrations.Core
{
    /// <summary>
    /// Class model that represents a table in database
    /// </summary>
    public class TableMapper
    {
        /// <summary>
        /// Table name (class name)
        /// </summary>
        public string TableName { get; private set; }
        /// <summary>
        /// Properties from a class that represent the columns in database
        /// </summary>
        public Columns[] Columns { get; private set; }
        private TableMapper()
        {
        }

        internal static TableMapper FromType<T>()
        {
            var table = typeof(T);
            var cols = ColumnMapper.FromType<T>().Columns;

            var tm = new TableMapper
            {
                TableName = table.Name,
                Columns = cols,
            };

            return tm;
        }

    }
}
