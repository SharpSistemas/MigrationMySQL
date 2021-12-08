namespace Sharp.MySQL.Migrations.Core.Models
{
    /// <summary>
    /// Properties from a class that represent the columns in database
    /// </summary>
    public class Columns
    {
        /// <summary>
        /// Property name
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Is Primary Key
        /// </summary>
        public bool IsPk { get; set; }
        /// <summary>
        /// Is Auto Increment
        /// </summary>
        public bool IsAI { get; set; }
        /// <summary>
        /// Is Unique Index
        /// </summary>
        public bool IsUnique { get; set; }
        /// <summary>
        /// Is Not Null
        /// </summary>
        public bool IsNotNull { get; set; }
        /// <summary>
        /// Is Decimal Property
        /// </summary>
        public int DecimalPrecision { get; set; }
        /// <summary>
        /// Default value
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// Property type
        /// </summary>
        public TypeField TypeField { get; set; }
        /// <summary>
        /// Property size
        /// </summary>
        public int SizeField { get; set; }
    }
}
