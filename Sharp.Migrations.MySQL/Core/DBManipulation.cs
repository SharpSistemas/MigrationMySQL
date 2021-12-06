using Dapper;
using MySql.Data.MySqlClient;
using Sharp.Migrations.MySQL.Core.Queries;
using Sharp.Migrations.MySQL.Helpers;
using System.Data;
using static Sharp.Migrations.MySQL.Helpers.TableMapper;

namespace Sharp.Migrations.MySQL.Core
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
