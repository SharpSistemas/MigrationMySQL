namespace Sharp.Migrations.MySQL.Core.Models
{
    public class TableSchema
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public int SizeField { get; set; }
        public string Null { get; set; }
        public string Key { get; set; }
        public string Default { get; set; }
        public string Extra { get; set; }
    }
}
