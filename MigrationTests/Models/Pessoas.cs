using MigrationTests.Attributes;
using System;
using static MigrationTests.Helpers.Enums;

namespace MigrationTests.Models
{
    public class Pessoas
    {
        [PrimaryKey]
        [TypeFieldBD(TipoCampoBD.INT, NotNull = true)]
        public int Id { get; set; }

        [TypeFieldBD(TipoCampoBD.BINARY, 16)]
        [Unique]
        public Guid Uuid { get; set; }

        [TypeFieldBD(TipoCampoBD.NVARCHAR, 100, false, "'teste'")]
        public string Nome { get; set; }

        [TypeFieldBD(TipoCampoBD.CHAR, 11)]
        public string Cpf { get; set; }

        [TypeFieldBD(TipoCampoBD.CHAR, 15)]
        public string Rg { get; set; }

        [TypeFieldBD(TipoCampoBD.CHAR, 8)]
        public string Cep { get; set; }

        [TypeFieldBD(TipoCampoBD.NVARCHAR, 100)]
        public string Logradouro { get; set; }

        [TypeFieldBD(TipoCampoBD.NVARCHAR, 50)]
        public string Bairro { get; set; }

        [TypeFieldBD(TipoCampoBD.NVARCHAR, 100)]
        public string Cidade { get; set; }

        [TypeFieldBD(TipoCampoBD.CHAR, 2)]
        public string Uf { get; set; }

        [TypeFieldBD(TipoCampoBD.NVARCHAR, 50)]
        public string Numero { get; set; }

        [TypeFieldBD(TipoCampoBD.NVARCHAR, 50)]
        public string Complemento { get; set; }

        [TypeFieldBD(TipoCampoBD.CHAR, 11)]
        public string Telefone { get; set; }

        [TypeFieldBD(TipoCampoBD.CHAR)]
        public string TesteDescricao { get; set; }
    }
}
