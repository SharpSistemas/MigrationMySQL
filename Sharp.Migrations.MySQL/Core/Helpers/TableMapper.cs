
using Sharp.Migrations.MySQL.Core.Models;

namespace Sharp.Migrations.MySQL.Core.Helpers
{
    public class TableMapper
    {
        public string TableName { get; private set; }
        public Colunas[] Colunas { get; private set; }
        private TableMapper()
        {
        }

        public static TableMapper FromType<T>()
        {
            var table = typeof(T);
            var cols = ColumnMapper.FromType<T>().Colunas;

            var tm = new TableMapper
            {
                TableName = table.Name,
                Colunas = cols,
            };

            return tm;
        }

    }
}
