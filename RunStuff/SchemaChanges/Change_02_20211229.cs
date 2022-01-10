using Dapper;
using Sharp.MySQL;
using Sharp.MySQL.Migrations.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunStuff.SchemaChanges
{
    public class Change_02_20211229 : ISchemaChange
    {
        private ConnectionFactory factory;

        public int SchemaVersion => 2;

        public Status CanRun() => Status.Ok;

        public void Initialize(ConnectionFactory factory)
        {
            this.factory = factory;
        }

        public void Run()
        {
            using (var conn = factory.GetConnection())
            {
                conn.Execute(@"
INSERT INTO pedidos (
Uuid, DataHora, ValorPedido, QtdeItens, UuidCliente) VALUES(
@Uuid, @DataHora, @ValorPedido, @QtdeItens, @UuidCliente)
",
new
{
    Uuid = Guid.NewGuid(),
    DataHora = DateTime.Now,
    ValorPedido = 5487.2349M,
    QtdeItens = 4,
    UuidCliente = Guid.NewGuid(),
});
            }
        }
    }
}
