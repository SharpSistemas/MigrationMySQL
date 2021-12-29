using Dapper;
using RunStuff.Models;
using Sharp.MySQL.Migrations.Helpers;
using Sharp.MySQL.Migrations.TypesHandler;
using System;

namespace RunStuff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Create a factory and store in the D.I.
            var mySQLFactory = new Sharp.MySQL.ConnectionFactory("Server=127.0.0.1;Port=3306;Uid=root;Pwd=5501;Database=portalsharp");

            DapperHelper.MapMySqlGuidHandler();

            // Creata an migration instance
            var migration = new Sharp.MySQL.Migration(mySQLFactory);
            // Add or change tables
            var result = migration.AddModel<Pessoas>()
                                  .AddModel<Empresas>()
                                  .AddModel<Pedidos>()
                                  .AddChange<SchemaChanges.Change_01_20211220>()
                                  .AddChange<SchemaChanges.Change_02_20211229>()
                                  .AddChange<SchemaChanges.Change_03_20211229>()
                                  .Migrate();

            foreach (var r in result.tables)
            {
                Console.WriteLine(r.ColumnsAdded);
                Console.WriteLine(r.WasCreated);
                Console.WriteLine(r.WasModified);
            }
        }
    }
}
