using System;

namespace Sharp.MySQL.Migrations.Attributes
{
    /// <summary>
    /// Defines a property as PrimaryKey. It's recommended to use with an INT property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
