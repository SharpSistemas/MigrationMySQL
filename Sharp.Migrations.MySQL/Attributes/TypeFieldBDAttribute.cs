using Sharp.MySQL.Migrations.Core.Models;
using Sharp.MySQL.Migrations.Exceptions;
using System;

namespace Sharp.MySQL.Migrations.Attributes
{
    /// <summary>
    /// Configure the fields in database.
    /// </summary>
    public class TypeFieldBD : Attribute
    {
        /// <summary>
        /// Sets the field type
        /// </summary>
        public TypeField TypeField { get; set; }
        /// <summary>
        /// Sets the field length
        /// </summary>
        public int? SizeField { get; set; }
        /// <summary>
        /// Define if it will be NOT NULL if true or NULL if false
        /// </summary>
        public bool NotNull { get; set; }
        /// <summary>
        /// Sets the default value expression to be setted in the database
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Configure the fields in database.
        /// </summary>
        /// <param name="typeField">Sets the field type</param>
        public TypeFieldBD(TypeField typeField)
        {
            if (!isValidTipoCampoBD(typeField)) throw new InvalidAttributeException($"Invalid Attribute type! {typeField}");
            TypeField = typeField;
        }
        /// <summary>
        /// Configure the fields in database. 
        /// NotNull is set to False
        /// </summary>
        /// <param name="typeField">Sets the field type</param>
        /// <param name="size">Sets the field length</param>
        public TypeFieldBD(TypeField typeField, int size)
        {
            if (!isValidTipoCampoBD(typeField)) throw new InvalidAttributeException($"Invalid Attribute type! {typeField}");
            TypeField = typeField;
            SizeField = size;
        }
        /// <summary>
        /// Configure the fields in database.
        /// </summary>
        /// <param name="typeField">Sets the field type</param>
        /// <param name="size">Sets the field length</param>
        /// <param name="notNull">Sets the default value to be stored in database if the field no be filled</param>
        /// <param name="defaultValue">Sets the default value to be stored in database if the field is not filled</param>
        public TypeFieldBD(TypeField typeField, int size, bool notNull, string defaultValue = null)
        {
            if (!isValidTipoCampoBD(typeField)) throw new InvalidAttributeException($"Invalid Attribute type! {typeField}");
            TypeField = typeField;
            SizeField = size;
            NotNull = notNull;
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
