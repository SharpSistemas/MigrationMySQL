namespace Sharp.Migrations.MySQL.Core.Helpers
{
    public class TableResult
    {
        public bool WasCreated { get; set; }
        public bool WasModified { get; set; }
        public int ColumnsAdded { get; set; }
    }
}
