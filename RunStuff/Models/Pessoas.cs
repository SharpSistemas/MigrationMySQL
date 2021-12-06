using Sharp.Migrations.MySQL.Attributes;
using Sharp.Migrations.MySQL.Core.Helpers;
using System;

namespace RunStuff.Models
{
    public class Pessoas
    {
        [PrimaryKey]
        [AutoIncrement]
        [TypeFieldBD(TipoCampoBD.INT, NotNull = true)]
        public int Id { get; set; }
        [Unique]
        [TypeFieldBD(TipoCampoBD.BINARY, 16)]
        public Guid Uuid { get; set; }
        [TypeFieldBD(TipoCampoBD.VARCHAR, 60, false)]
        public string Nome { get; set; }
    }
}
