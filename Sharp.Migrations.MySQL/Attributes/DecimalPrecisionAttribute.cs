﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sharp.MySQL.Migrations.Attributes
{
    /// <summary>
    /// Attribute for decimal types to define the size and precision.
    /// </summary>
    public class DecimalPrecisionAttribute : Attribute
    {
        /// <summary>
        /// Number of spaces after comma (precision). This value takes spaces from Length (TypeFieldBD Size).
        /// </summary>
        public int DecimalSpaces { get; set; }

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
