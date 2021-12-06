using System;

namespace Sharp.Migrations.MySQL.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        public string Name { get; set; }

        public NameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
