using System;

namespace Sharp.MySQL.Migrations.Attributes
{
    /// <summary>
    /// Attribute for decimal types to define the precision.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalPrecisionAttribute : Attribute
    {
        /// <summary>
        /// Number of spaces after comma (precision). This value takes spaces from Length (TypeFieldBD Size).
        /// </summary>
        public int DecimalSpaces { get; private set; }

        /// <summary>
        /// Constructor of class
        /// </summary>
        /// <param name="decimalSpaces">Number of spaces after comma (precision). This value takes spaces from Length (TypeFieldBD Size).</param>
        public DecimalPrecisionAttribute(int decimalSpaces)
        {
            DecimalSpaces = decimalSpaces;
        }
    }
}
