using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharp.MySQL.Migrations.TypesHandler
{
    public class MySqlGuidHandler : SqlMapper.TypeHandler<Guid>
    {
        public override Guid Parse(object value)
        {
            return new Guid((byte[])value);
        }
        public override void SetValue(System.Data.IDbDataParameter parameter, Guid value) => parameter.Value = value.ToByteArray();
    }
}
