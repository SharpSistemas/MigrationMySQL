
using Sharp.MySQL.Migrations.Core.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Sharp.MySQL.Migrations.Core
{
    /// <summary>
    /// Models classes mapper attributes
    /// </summary>
    public class ColumnMapper
    {
        /// <summary>
        /// Properties from the class
        /// </summary>
        public Columns[] Columns { get; private set; }

        private ColumnMapper() { }

        internal static ColumnMapper FromType<T>()
        {
            var properties = typeof(T).GetProperties();
            var columns = new Columns[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                var ignoredAttribute = properties[i].GetCustomAttributes<Attributes.IgnoreAttribute>()
                                                    .FirstOrDefault();
                if (ignoredAttribute != null) continue;

                string nameField = properties[i].Name;
                int? sizeField;
                int? decimalSpaces;

                var nameAttribute = properties[i].GetCustomAttributes<Attributes.NameAttribute>()
                                                 .FirstOrDefault();

                var typeField = properties[i].GetCustomAttributes<Attributes.TypeFieldBD>()
                                             .FirstOrDefault();

                var decimalPrecisionAttribute = properties[i].GetCustomAttributes<Attributes.DecimalPrecisionAttribute>()
                                                             .FirstOrDefault();

                if (nameAttribute != null) nameField = nameAttribute.Name;

                if (typeField.SizeField == null) sizeField = null;
                else sizeField = typeField.SizeField;

                if (decimalPrecisionAttribute == null) decimalSpaces = null;
                else decimalSpaces = decimalPrecisionAttribute.DecimalSpaces;

                if (typeField == null) throw new Exceptions.NullAttributeException($"No field definition. Decorate it with 'TypeFieldBD'. Field: {properties[i].Name}");

                columns[i] = new Columns
                {
                    NameField = nameField,
                    IsPk = Attribute.IsDefined(properties[i], typeof(Attributes.PrimaryKeyAttribute)),
                    IsUnique = Attribute.IsDefined(properties[i], typeof(Attributes.UniqueAttribute)),
                    IsAI = Attribute.IsDefined(properties[i], typeof(Attributes.AutoIncrementAttribute)),
                    TypeField = typeField.TypeField,
                    NotNull = typeField.NotNull,
                    DefaultValue = typeField.DefaultValue,
                    SizeField = sizeField,
                    DecimalPrecision = decimalSpaces,
                };
            }

            var cm = new ColumnMapper
            {
                Columns = columns,
            };

            return cm;
        }
    }
}
