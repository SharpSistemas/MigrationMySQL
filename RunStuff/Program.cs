using RunStuff.Models;
using Sharp.Migrations.MySQL;
using System;

namespace RunStuff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var dbFac = new ConnectionFactory("Server=127.0.0.1;Port=3306;Uid=root;Pwd=5501;Database=portalsharp");
            var mig = getMigration(dbFac);

            mig.Add<Pessoas>()
               .Migrate();
        }
        private static Migration getMigration(ConnectionFactory dbFac) => new Migration(dbFac);
    }
}
