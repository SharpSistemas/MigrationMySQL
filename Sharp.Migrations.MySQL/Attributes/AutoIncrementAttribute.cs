using System;

namespace Sharp.MySQL.Migrations.Attributes
{
    /// <summary>
    /// Defines a property as AutoIncrement. Property MUST be INT and NotNull (see about TypeFieldBDAttribute to set as NotNull).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncrementAttribute : Attribute
    {
    }
}
