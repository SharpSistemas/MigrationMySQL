
using Sharp.MySQL.Migrations.Core.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Sharp.MySQL.Migrations.Core
{
    public class ColumnMapper
    {
        public Columns[] Columns { get; private set; }

        private ColumnMapper() { }

        internal static ColumnMapper FromType<T>()
        {
            var properties = typeof(T).GetProperties();
            var columns = new Columns[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                var typeField = properties[i].GetCustomAttributes<Attributes.TypeFieldBD>()
                                             .FirstOrDefault();

                if (typeField == null) throw new Exceptions.NullAttributeException($"No field definition. Decorate it with 'TypeFieldBD'. Field: {properties[i].Name}");

                columns[i] = new Columns
                {
                    FieldName = properties[i].Name,
                    IsPk = Attribute.IsDefined(properties[i], typeof(Attributes.PrimaryKeyAttribute)),
                    IsUnique = Attribute.IsDefined(properties[i], typeof(Attributes.UniqueAttribute)),
                    IsAI = Attribute.IsDefined(properties[i], typeof(Attributes.AutoIncrementAttribute)),
                    SizeField = typeField.SizeField,
                    TypeField = typeField.TypeField,
                    IsNotNull = typeField.NotNull,
                    DefaultValue = typeField.DefaultValue,
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
