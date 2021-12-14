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
            var mySQLFactory = new Sharp.MySQL.ConnectionFactory("connstring");
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
