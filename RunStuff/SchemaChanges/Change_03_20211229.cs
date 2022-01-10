using Dapper;
using Sharp.MySQL;
using Sharp.MySQL.Migrations.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunStuff.SchemaChanges
{
    public class Change_03_20211229 : ISchemaChange
    {
        private ConnectionFactory factory;

        public int SchemaVersion => 3;
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
DROP PROCEDURE IF EXISTS sp_ConsultarEmpresa;
CREATE PROCEDURE sp_ConsultarEmpresa (IN uuid BINARY(16))
BEGIN
SELECT * FROM empresas
WHERE empresas.uuid=uuid;
END;");
            }
        }
    }
}
