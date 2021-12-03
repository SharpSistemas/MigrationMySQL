using static MigrationTests.Helpers.Enums;

namespace MigrationTests.Core.Models
{
    public class Colunas
    {
        public string FieldName { get; set; }
        public bool IsPk { get; set; }
        public bool IsAI { get; set; }
        public bool IsUnique { get; set; }
        public bool IsNotNull { get; set; }
        public string DefaultValue { get; set; }
        public TipoCampoBD TypeField { get; set; }
        public int SizeField { get; set; }
    }
}
