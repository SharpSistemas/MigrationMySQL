using System;

namespace Sharp.MySQL.Migrations.Attributes
{
    /// <summary>
    /// Defines a property as Unique INDEX.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : Attribute
    {
    }
}
