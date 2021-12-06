using Sharp.Migrations.MySQL.Core.Helpers;
using Sharp.Migrations.MySQL.Exceptions;
using System;

namespace Sharp.Migrations.MySQL.Attributes
{
    public class TypeFieldBD : Attribute
    {
        public TipoCampoBD TipoCampo { get; set; }
        public int Tamanho { get; set; }
        public bool NotNull { get; set; }
        public string DefaultValue { get; set; }

        public TypeFieldBD(TipoCampoBD tipoCampo)
        {
            if (!isValidTipoCampoBD(tipoCampo)) throw new InvalidAttributeException($"Invalid Attribute type! {tipoCampo}");
            TipoCampo = tipoCampo;
        }

        public TypeFieldBD(TipoCampoBD tipoCampo, int tamanho)
        {
            if (!isValidTipoCampoBD(tipoCampo)) throw new InvalidAttributeException($"Invalid Attribute type! {tipoCampo}");
            TipoCampo = tipoCampo;
            Tamanho = tamanho;
        }
        public TypeFieldBD(TipoCampoBD tipoCampo, int tamanho, bool isNullable, string defaultValue = null)
        {
            if (!isValidTipoCampoBD(tipoCampo)) throw new InvalidAttributeException($"Invalid Attribute type! {tipoCampo}");
            TipoCampo = tipoCampo;
            Tamanho = tamanho;
            NotNull = isNullable;
            DefaultValue = defaultValue;
        }
        private bool isValidTipoCampoBD(TipoCampoBD tipoCampo)
        {
            if (tipoCampo == TipoCampoBD.NOT_SET) return false;

            if (!Enum.IsDefined(typeof(TipoCampoBD), tipoCampo)) return false;

            return true;
        }
    }
}
