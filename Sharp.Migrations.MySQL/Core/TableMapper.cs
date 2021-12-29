
using Sharp.MySQL.Migrations.Core.Models;
using System;
using System.Linq;

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
            var tableName = table.Name;
            var cols = ColumnMapper.FromType<T>().Columns;

            var nameAttr = Attribute.GetCustomAttributes(table).FirstOrDefault(o => o.GetType() == typeof(Attributes.NameAttribute));

            if (nameAttr != null) tableName = ((Attributes.NameAttribute)nameAttr).Name;

            var tm = new TableMapper
            {
                TableName = tableName,
                Columns = cols,
            };

            return tm;
        }

    }
}
