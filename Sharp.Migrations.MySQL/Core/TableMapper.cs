
using Sharp.MySQL.Migrations.Core.Models;

namespace Sharp.MySQL.Migrations.Core
{
    public class TableMapper
    {
        public string TableName { get; private set; }
        public Columns[] Columns { get; private set; }
        private TableMapper()
        {
        }

        public static TableMapper FromType<T>()
        {
            var table = typeof(T);
            var cols = ColumnMapper.FromType<T>().Columns;

            var tm = new TableMapper
            {
                TableName = table.Name,
                Columns = cols,
            };

            return tm;
        }

    }
}
