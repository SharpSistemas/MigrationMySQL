using Sharp.MySQL.Migrations.Attributes;
using Sharp.MySQL.Migrations.Core.Models;
using System;

namespace RunStuff.Models
{
    public class Pessoas
    {
        [PrimaryKey]
        [TypeFieldBD(TypeField.INT, NotNull = true)]
        [AutoIncrement]
        public int Codigo { get; set; } //INT AI PK NN

        [TypeFieldBD(TypeField.BINARY, 16, NotNull = true)]
        [Unique]
        public Guid Uuid { get; set; } //BINARY(16) UQ NN

        [TypeFieldBD(TypeField.NVARCHAR, 100, NotNull = true)]
        public string Nome { get; set; } //NVARCHAR(100) NN

        [TypeFieldBD(TypeField.CHAR, 8, NotNull = true)]
        public string CEP { get; set; } //CHAR(8) NN

        [TypeFieldBD(TypeField.NVARCHAR, 100, NotNull = true)]
        public string Logradouro { get; set; } //NVARCHAR(100) NN

        [TypeFieldBD(TypeField.NVARCHAR, 50, NotNull = true)]
        public string Bairro { get; set; } //NVARCHAR (50) NN

        [TypeFieldBD(TypeField.NVARCHAR, 50, NotNull = true)]
        public string Cidade { get; set; } //NVARCHAR (50) NN

        [TypeFieldBD(TypeField.CHAR, 2, NotNull = true)]
        public string UF { get; set; } //CHAR(2) NN

        [TypeFieldBD(TypeField.NVARCHAR, 30, NotNull = true)]
        public string Numero { get; set; } //NVARCHAR(30) NN

        [TypeFieldBD(TypeField.NVARCHAR, 50, DefaultValue = "'NÃO INFORMADO'")]
        public string Complemento { get; set; } //NVARCHAR(50) DEFAULT VALUE 'NÃO INFORMADO'

        [TypeFieldBD(TypeField.DECIMAL, 12, NotNull = true)]
        [DecimalPrecision(3)]
        public decimal Salario { get; set; } //DECIMAL(12,3)

        [TypeFieldBD(TypeField.DATETIME)]
        public DateTime Nascimento { get; set; } //DATETIME NN

        [TypeFieldBD(TypeField.DATE)]
        public DateTime Contratacao { get; set; } //DATE NN

        [TypeFieldBD(TypeField.TIME)]
        public DateTime CargaHoraria { get; set; } //TIME NN
    }
}
