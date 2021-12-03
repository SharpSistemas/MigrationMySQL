using MigrationTests.Models;
using System;

namespace MigrationTests
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Iniciando...");

            var tableResultPessoas = Core.Helpers.CriaModificaTabela<Pessoas>();
            
            Console.WriteLine("Fim do processo!");
        }
    }
}
