using Sharp.MySQL.Migrations.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunStuff.Models
{
    public class Empresas
    {
        [TypeFieldBD(TypeField.INT, notNull: true)]
        public int Id { get; set; }

        [TypeFieldBD(TypeField.BINARY, 16)]
        public Guid Uuid { get; set; }

        [TypeFieldBD(TypeField.BINARY, size: 16, defaultValue: "'nue'", notNull: true)]
        public string Nome { get; set; }
    }
}
