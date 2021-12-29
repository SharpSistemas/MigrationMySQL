using System;
using Dapper;
using Sharp.MySQL;
using Sharp.MySQL.Migrations.Core;

namespace RunStuff.SchemaChanges
{
    public class Change_01_20211220 : ISchemaChange
    {
        private ConnectionFactory factory;

        public int SchemaVersion => 1;

        public void Initialize(ConnectionFactory factory)
        {
            this.factory = factory;
        }

        public bool CanRun()
        {
            return true;
        }

        public void Run()
        {
            using (var cnn = factory.GetConnection())
            {
                cnn.Execute(@"
INSERT INTO pessoas(
Uuid, Nome, CEP, Logradouro, Bairro, Cidade, UF, Numero, Complemento, Salario, Nascimento, Contratacao, CargaHoraria) VALUES (
@Uuid, @Nome, @CEP, @Logradouro, @Bairro, @Cidade, @UF, @Numero, @Complemento, @Salario, @Nascimento, @Contratacao, @CargaHoraria)",
new
{
    Uuid = Guid.NewGuid(),
    Nome = "Teste Nome",
    CEP = "12345700",
    Logradouro = "Rua das Orquídeas",
    Bairro = "João das Neves",
    Cidade = "Pé de Pano",
    UF = "SP",
    Numero = "Sem Numero",
    Complemento = "Apartamento 91",
    Salario = 2000.43M,
    Nascimento = DateTime.Parse("23/05/1987 13:44:20"),
    Contratacao = "2021-05-17",
    CargaHoraria = "220:00",
});
            }
        }
    }
}
