﻿namespace Sharp.Migrations.MySQL.Core.Models
{
    public class Columns
    {
        public string FieldName { get; set; }
        public bool IsPk { get; set; }
        public bool IsAI { get; set; }
        public bool IsUnique { get; set; }
        public bool IsNotNull { get; set; }
        public string DefaultValue { get; set; }
        public TypeField TypeField { get; set; }
        public int SizeField { get; set; }
    }
}
