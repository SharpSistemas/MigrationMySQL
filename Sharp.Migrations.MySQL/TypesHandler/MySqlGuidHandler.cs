using Dapper;
using System;

namespace Sharp.MySQL.Migrations.TypesHandler
{
    /// <summary>
    /// Guid handler for mysqls db
    /// </summary>
    public class MySqlGuidHandler : SqlMapper.TypeHandler<Guid>
    {
        /// <summary>
        /// Parsers a binary value to guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Guid Parse(object value)
        {
            return new Guid((byte[])value);
        }

        /// <summary>
        /// Parsers a guid value to binary
        /// </summary>
        /// <param name="parameter">Object in bytearray</param>
        /// <param name="value">Guid to be converted in bytearray</param>
        public override void SetValue(System.Data.IDbDataParameter parameter, Guid value) => parameter.Value = value.ToByteArray();
    }
}
