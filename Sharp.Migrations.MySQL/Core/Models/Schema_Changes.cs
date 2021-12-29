using Sharp.MySQL.Migrations.Attributes;
using System;

namespace Sharp.MySQL.Migrations.Core.Models
{
    /// <summary>
    /// Table in database to store schema changes
    /// </summary>
    public class Schema_Changes
    {
        /// <summary>
        /// Id of schema
        /// </summary>
        [PrimaryKey]
        [TypeFieldBD(typeField: TypeField.INT, notNull: true)]
        [AutoIncrement]
        public int Schema_Id { get; set; }

        /// <summary>
        /// Schema version
        /// </summary>
        [TypeFieldBD(typeField: TypeField.INT, notNull: true)]
        public string Schema_Version { get; set; }

        /// <summary>
        /// Datetime when the change occured
        /// </summary>
        [TypeFieldBD(typeField: TypeField.DATETIME, notNull: true)]
        public DateTime Schema_Changed { get; set; }
    }
}
