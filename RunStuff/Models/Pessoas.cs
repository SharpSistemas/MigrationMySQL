using Sharp.MySQL.Migrations.Attributes;
using System;

namespace RunStuff.Models
{
    [Name("Pessoas")]
    public class Pessoas
    {
        [PrimaryKey]
        [TypeFieldBD(TypeField.INT, notNull: true)]
        [AutoIncrement]
        [Name("Codguin")]
        public int Codigo { get; set; } //INT AI PK NN

        [TypeFieldBD(TypeField.BINARY, 16, notNull: true)]
        [Unique]
        public Guid Uuid { get; set; } //BINARY(16) UQ NN

        [TypeFieldBD(TypeField.NVARCHAR, 100, notNull: true)]
        public string Nome { get; set; } //NVARCHAR(100) NN

        [TypeFieldBD(TypeField.CHAR, 8, notNull: true)]
        public string CEP { get; set; } //CHAR(8) NN

        [TypeFieldBD(TypeField.NVARCHAR, 100, notNull: true)]
        public string Logradouro { get; set; } //NVARCHAR(100) NN

        [TypeFieldBD(TypeField.NVARCHAR, 50, notNull: true)]
        public string Bairro { get; set; } //NVARCHAR (50) NN

        [TypeFieldBD(TypeField.NVARCHAR, 50, notNull: true)]
        public string Cidade { get; set; } //NVARCHAR (50) NN

        [TypeFieldBD(TypeField.CHAR, 2, notNull: true)]
        public string UF { get; set; } //CHAR(2) NN

        [TypeFieldBD(TypeField.NVARCHAR, 30, notNull: true)]
        public string Numero { get; set; } //NVARCHAR(30) NN

        [TypeFieldBD(TypeField.NVARCHAR, 50, defaultValue: "'NÃO INFORMADO'")]
        public string Complemento { get; set; } //NVARCHAR(50) DEFAULT VALUE 'NÃO INFORMADO'

        [TypeFieldBD(TypeField.DECIMAL, 12, notNull: true)]
        [DecimalPrecision(3)]
        public decimal Salario { get; set; } //DECIMAL(12,3)

        [TypeFieldBD(TypeField.DATETIME)]
        public DateTime Nascimento { get; set; } //DATETIME NN

        [TypeFieldBD(TypeField.DATE)]
        public DateTime Contratacao { get; set; } //DATE NN

        [TypeFieldBD(TypeField.TIME)]
        public DateTime CargaHoraria { get; set; } //TIME NN

        [TypeFieldBD(TypeField.VARCHAR, 100, notNull: false, defaultValue: "'54'")]
        public DateTime Fock { get; set; } //TIME NN

        [TypeFieldBD(TypeField.NVARCHAR, size: 50, notNull: true, defaultValue: "'FTS'")]
        public string TesteGetDate1 { get; set; }

        [TypeFieldBD(TypeField.NVARCHAR, size: 50, notNull: true, defaultValue: "'SELECT CURDATE()'")]
        public string TesteGetDate2 { get; set; }
    }
}
