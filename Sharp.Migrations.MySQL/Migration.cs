using Dapper;
using Sharp.MySQL.Migrations.Core;
using Sharp.MySQL.Migrations.Core.Models;
using Sharp.MySQL.Migrations.Core.Queries;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sharp.MySQL
{
    public class Migration
    {
        private ConnectionFactory dbFac { get; }
        private List<TableMapper> tables;
        public Migration(ConnectionFactory dbFac)
        {
            this.dbFac = dbFac;
            tables = new List<TableMapper>();
        }
        public Migration Add<T>()
        {
            tables.Add(TableMapper.FromType<T>());
            return this;
        }
        public TableResult[] Migrate()
        {
            var result = tables.Select(t => migrateTable(t)).ToArray();
            tables.Clear();
            return result;
        }
        private TableResult migrateTable(TableMapper tableMapper)
        {
            if (!existeTabela(tableMapper.TableName)) return CreateTable(tableMapper);
            return ModifyTable(tableMapper);
        }
        private TableResult CreateTable(TableMapper tableMapper)
        {
            string query = BuildQ.buildQueryCreateTable(tableMapper);
            using (var db = dbFac.GetConnection())
            {
                var result = db.Execute(query, db);
                return new TableResult
                {
                    ColumnsAdded = tableMapper.Columns.Length,
                    WasCreated = true,
                    WasModified = false,
                };
            }
        }
        private TableResult ModifyTable(TableMapper tbMapper)
        {
            var colunasBD = getTableSchema(tbMapper.TableName);
            var query = BuildQ.buildQueryAlterTable(tbMapper, colunasBD);

            using (var db = dbFac.GetConnection())
            {
                var result = db.Execute(query, db);

                return new TableResult
                {
                    ColumnsAdded = tbMapper.Columns.Length - colunasBD.Length,
                    WasModified = true,
                    WasCreated = false,
                };
            }
        }
        private bool existeTabela(string tableName)
        {
            using (var db = dbFac.GetConnection())
            {
                var tb = db.QueryFirstOrDefault("show tables like @tableName", new { tableName });
                return tb != null;
            }
        }
        private TableIndex[] getTableIndexes(string tableName)
        {
            using (var db = dbFac.GetConnection())
            {
                var indexes = db.Query<TableIndex>($"show index from {tableName}", new { tableName }).ToArray();
                return indexes;
            }
        }
        private TableSchema[] getTableSchema(string tableName)
        {
            if (!existeTabela(tableName)) return null;
            using (var db = dbFac.GetConnection())
            {
                var tb = db.Query<TableSchema>($"describe {tableName}").ToArray();

                foreach (var t in tb)
                {
                    if (!t.Type.Contains('(') || !t.Type.Contains(')')) continue;

                    int start = t.Type.IndexOf('(') + 1;
                    int end = t.Type.IndexOf(')');

                    if (start > end) throw new System.InvalidOperationException($"Field type retrieved from MySQL was in an incorret format! {t}");

                    if (!int.TryParse(t.Type.Substring(start, end - start), out int result)) continue;

                    t.SizeField = result;
                    t.Type = t.Type.Substring(0, start - 1);
                }
                return tb;
            }
        }
    }
}
