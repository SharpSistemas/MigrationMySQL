using Dapper;
using Sharp.MySQL.Migrations.TypesHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharp.MySQL.Migrations.Helpers
{
    /// <summary>
    /// Class with dapper helpers
    /// </summary>
    public static class DapperHelper
    {
        /// <summary>
        /// Maps a handler to Guids in MySQL, which allows to store guids as BINARY
        /// </summary>
        public static void MapMySqlGuidHandler()
        {
            SqlMapper.AddTypeHandler(new MySqlGuidHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
        }
    }
}
