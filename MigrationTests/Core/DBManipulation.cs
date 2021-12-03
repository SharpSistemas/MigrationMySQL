using Dapper;
using MigrationTests.Core.Queries;
using MigrationTests.Helpers;
using MySql.Data.MySqlClient;
using System.Data;
using static MigrationTests.Helpers.TableMapper;

namespace MigrationTests.Core
{
    public static class DBManipulation
    {
        public static TableResult CriaTabela(TableMapper tableMapper)
        {
            string query = BuildQ.buildQueryCreateTable(tableMapper);
            using (var db = Connect(""))
            {
                var result = db.Execute(query, db);
                return new TableResult
                {
                    ColumnsAdded = tableMapper.Colunas.Length,
                    WasCreated = true,
                    WasModified = false,
                };
            }
        }
        public static TableResult ModificaTabela(TableMapper tbMapper)
        {
            var query = BuildQ.buildQueryAlterTable(tbMapper);

            if (string.IsNullOrWhiteSpace(query)) return new TableResult
            {
                ColumnsAdded = 0,
                WasCreated = false,
                WasModified = false,
            };

            using (var db = Connect(""))
            {
                var result = db.Execute(query, db);

                return new TableResult
                {

                };
            }
        }
        public static IDbConnection Connect(string connString)
        {
            connString = "Server=127.0.0.1;Port=3306;Uid=root;Pwd=5501;Database=portalsharp";

            var conn = new MySqlConnection(connString);
            conn.Open();
            return conn;
        }
    }
}
