using RunStuff.Models;
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
            // Creata an migration instance
            var migration = new Sharp.MySQL.Migration(mySQLFactory);
            // Add or change tables
            var result = migration.Add<Pessoas>()
                                  .Migrate();

            foreach (var r in result)
            {
                Console.WriteLine(r.ColumnsAdded);
                Console.WriteLine(r.WasCreated);
                Console.WriteLine(r.WasModified);
            }
        }
    }
}
