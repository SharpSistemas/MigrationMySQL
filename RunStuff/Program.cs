using Dapper;
using RunStuff.Models;
using RunStuff.SchemaChanges;
using Sharp.MySQL.Migrations.Helpers;
using Sharp.MySQL.Migrations.TypesHandler;
using System;
using System.IO;

namespace RunStuff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!File.Exists("connString.cn")) throw new FileNotFoundException("File with connection string not found");
            if (new FileInfo("connString.cn").Length == 0) throw new InvalidOperationException("Empty connection string");

            var connString = File.ReadAllLines("connString.cn")[0];

            // Create a factory and store in the D.I.
            var mySQLFactory = new Sharp.MySQL.ConnectionFactory(connString);

            DapperHelper.MapMySqlGuidHandler();

            // Creata an migration instance
            var migration = new Sharp.MySQL.Migration(mySQLFactory);
            // Add or change tables

            migration.AddModel<Empresas>()
                     .AddModel<Pessoas>()
                     .AddModel<Pedidos>();

            migration.AddChange<Change_01_20211220>()
                     .AddChange<Change_02_20211229>()
                     .AddChange<Change_03_20211229>();

            var start = DateTime.Now; 
            var result = migration.Migrate();
            var ts = DateTime.Now - start;

            Console.WriteLine($"Duration: {ts}");
            foreach (var r in result.tables)
            {
                Console.WriteLine($"Table: {r.TableName}");
                Console.WriteLine($"Columns added: {r.ColumnsAdded}");
                Console.WriteLine($"Table Created: {r.WasCreated}");
                Console.WriteLine($"Table Modified: {r.WasModified}");
                Console.WriteLine("---------------------------------");
            }
        }
    }
}
