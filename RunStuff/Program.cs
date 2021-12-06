using RunStuff.Models;
using System;

namespace RunStuff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var mySQLFactory = new Sharp.MySQL.ConnectionFactory("yourConnectionString");
            var migration = new Sharp.MySQL.Migration(mySQLFactory);

            var result = migration.Add<Pessoas>()
                                  .Add<Pessoas>()
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
