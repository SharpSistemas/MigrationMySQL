using Sharp.MySQL.Migrations.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunStuff.Models
{
    public class Pedidos
    {
        [PrimaryKey]
        [TypeFieldBD(TypeField.INT, NotNull = true)]
        public int Id { get; set; }

        [Unique]
        [TypeFieldBD(TypeField.BINARY, size: 16)]
        public Guid Uuid { get; set; }
        
        [TypeFieldBD(TypeField.DATETIME)]
        public DateTime DataHora { get; set; }
        
        [TypeFieldBD(TypeField.DECIMAL, size: 9)]
        [DecimalPrecision(5)]
        public decimal ValorPedido { get; set; }
        
        [TypeFieldBD(TypeField.INT)]
        public int QtdeItens { get; set; }
        
        [TypeFieldBD(TypeField.BINARY, size: 16)]
        public Guid UuidCliente { get; set; }
    }
}
