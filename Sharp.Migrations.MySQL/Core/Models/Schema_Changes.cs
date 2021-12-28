using Sharp.MySQL.Migrations.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharp.MySQL.Migrations.Core.Models
{
    public class Schema_Changes
    {
        [PrimaryKey]
        [TypeFieldBD(typeField: TypeField.INT, NotNull = true)]
        [AutoIncrement]
        public int Schema_Id { get; set; }

        [TypeFieldBD(typeField: TypeField.INT, NotNull = true)]
        public string Schema_Version { get; set; }
        [TypeFieldBD(TypeField.DATETIME)]
        public DateTime Schema_Changed { get; set; }
    }
}
