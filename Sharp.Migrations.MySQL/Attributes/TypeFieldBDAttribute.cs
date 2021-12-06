using Sharp.MySQL.Migrations.Core.Models;
using Sharp.MySQL.Migrations.Exceptions;
using System;

namespace Sharp.MySQL.Migrations.Attributes
{
    public class TypeFieldBD : Attribute
    {
        public TypeField TypeField { get; set; }
        public int SizeField { get; set; }
        public bool NotNull { get; set; }
        public string DefaultValue { get; set; }

        public TypeFieldBD(TypeField typeField)
        {
            if (!isValidTipoCampoBD(typeField)) throw new InvalidAttributeException($"Invalid Attribute type! {typeField}");
            TypeField = typeField;
        }

        public TypeFieldBD(TypeField typeField, int size)
        {
            if (!isValidTipoCampoBD(typeField)) throw new InvalidAttributeException($"Invalid Attribute type! {typeField}");
            TypeField = typeField;
            SizeField = size;
        }
        public TypeFieldBD(TypeField typeField, int size, bool isNullable, string defaultValue = null)
        {
            if (!isValidTipoCampoBD(typeField)) throw new InvalidAttributeException($"Invalid Attribute type! {typeField}");
            TypeField = typeField;
            SizeField = size;
            NotNull = isNullable;
            DefaultValue = defaultValue;
        }
        private bool isValidTipoCampoBD(TypeField typeField)
        {
            if (typeField == TypeField.NOT_SET) return false;

            if (!Enum.IsDefined(typeof(TypeField), typeField)) return false;

            return true;
        }
    }
}
