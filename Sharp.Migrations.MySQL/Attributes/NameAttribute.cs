using System;

namespace Sharp.MySQL.Migrations.Attributes
{
    /// <summary>
    /// Attribute to set the name of a class or field. If not set, the name will be the property or class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        /// <summary>
        /// Name to be set
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Constructor of class
        /// </summary>
        /// <param name="name">Name to be set</param>
        public NameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
