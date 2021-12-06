
using Sharp.Migrations.MySQL.Core.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Sharp.Migrations.MySQL.Helpers
{
    public class ColumnMapper
    {
        public Colunas[] Colunas { get; private set; }

        private ColumnMapper() { }

        public static ColumnMapper FromType<T>()
        {
            var properties = typeof(T).GetProperties();
            var colunas = new Colunas[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                var typeField = properties[i].GetCustomAttributes<Attributes.TypeFieldBD>()
                                             .FirstOrDefault();

                if (typeField == null) throw new Exceptions.NullAttributeException($"No field definition. Decorate it with 'TypeFieldBD'. Field: {properties[i].Name}");

                colunas[i] = new Colunas
                {
                    FieldName = properties[i].Name,
                    IsPk = Attribute.IsDefined(properties[i], typeof(Attributes.PrimaryKeyAttribute)),
                    IsUnique = Attribute.IsDefined(properties[i], typeof(Attributes.UniqueAttribute)),
                    IsAI = Attribute.IsDefined(properties[i], typeof(Attributes.AutoIncrementAttribute)),
                    SizeField = typeField.Tamanho,
                    TypeField = typeField.TipoCampo,
                    IsNotNull = typeField.NotNull,
                    DefaultValue = typeField.DefaultValue,
                };
            }

            var cm = new ColumnMapper
            {
                Colunas = colunas,
            };

            return cm;
        }
    }
}
