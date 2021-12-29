using System;

namespace Sharp.MySQL.Migrations.Attributes
{
    /// <summary>
    /// Attribute to bem used to exclude fields from migration
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}
