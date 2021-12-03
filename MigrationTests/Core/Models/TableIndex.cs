namespace MigrationTests.Core.Models
{
    public class TableIndex
    {
        public string Table { get; set; }
        public string Non_unique { get; set; }
        public string Key_name { get; set; }
        public string Seq_in_index { get; set; }
        public string Column_name { get; set; }
        public string Collation { get; set; }
        public string Cardinality { get; set; }
        public string Sub_part { get; set; }
        public string Packed { get; set; }
        public string Null { get; set; }
        public string Index_Type { get; set; }
        public string Comment { get; set; }
        public string Index_comment { get; set; }
        public string Visible { get; set; }
        public string Expression { get; set; }
    }
}
