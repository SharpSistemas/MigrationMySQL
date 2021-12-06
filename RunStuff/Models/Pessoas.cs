using Sharp.MySQL.Migrations.Attributes;
using Sharp.MySQL.Migrations.Core.Models;
using System;

namespace RunStuff.Models
{
    public class Pessoas
    {
        [PrimaryKey]
        [AutoIncrement]
        [TypeFieldBD(TypeField.INT, NotNull = true)]
        public int Id { get; set; }
        [Unique]
        [TypeFieldBD(TypeField.BINARY, 16)]
        public Guid Uuid { get; set; }
        [TypeFieldBD(TypeField.VARCHAR, 60, false)]
        public string Nome { get; set; }
    }
}
