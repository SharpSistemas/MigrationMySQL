using Dapper;
using Sharp.MySQL.Migrations.Core;
using Sharp.MySQL.Migrations.Core.Models;
using Sharp.MySQL.Migrations.Core.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sharp.MySQL
{
    /// <summary>
    /// Class that contains functions to migration
    /// </summary>
    public class Migration
    {
        private ConnectionFactory dbFac { get; }
        private List<TableMapper> tables;
        private Dictionary<int, ISchemaChange> schemaVersions;

        /// <summary>
        /// Constructor class
        /// </summary>
        /// <param name="dbFac">Connection factory</param>
        public Migration(ConnectionFactory dbFac)
        {
            this.dbFac = dbFac;
            tables = new List<TableMapper>();
        }

        [Obsolete]
        public Migration Add<T>() => AddModel<T>();
        /// <summary>
        /// Function to add models to migration
        /// </summary>
        /// <typeparam name="T">Model class</typeparam>
        /// <returns>A list of table mapper</returns>
        public Migration AddModel<T>()
        {
            tables.Add(TableMapper.FromType<T>());
            return this;
        }

        public Migration AddChange<T>() where T : ISchemaChange
        {
            ISchemaChange change = Activator.CreateInstance<T>();

            var version = change.SchemaVersion;
            if (schemaVersions.ContainsKey(version)) throw new InvalidOperationException($"Schema {version} is already defined");

            schemaVersions.Add(version, change);

            return this;
        }


        /// <summary>
        /// Fuction that executes the migration
        /// </summary>
        /// <returns>An array of table result</returns>
        public MigrationRresult Migrate()
        {
            // Migrate Tables
            var result = tables.Select(t => migrateTable(t)).ToArray();
            tables.Clear();

            if (schemaVersions.Count > 0)
            {
                // criar table schemaVersions? 
                migrateTable(TableMapper.FromType<Migrations.Core.Models.Schema_Changes>());
            }

            // Migrate Changes
            int minVersion = 0, maxVersion = 0;

            // if !CanRun exception


            return new MigrationRresult()
            {
                tables = result,
                FirstSchemaVersion = minVersion,
                LastSchemaVersion = maxVersion,
            };
        }

        private TableResult migrateTable(TableMapper tableMapper)
        {
            if (!existeTabela(tableMapper.TableName)) return CreateTable(tableMapper);
            return ModifyTable(tableMapper);
        }
        private TableResult CreateTable(TableMapper tableMapper)
        {
            string query = QueryBuilder.buildQueryCreateTable(tableMapper);
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
            var query = QueryBuilder.buildQueryAlterTable(tbMapper, colunasBD);

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
                    string type = t.Type.Substring(0, start - 1);

                    if (start > end) throw new System.InvalidOperationException($"Field type retrieved from MySQL was in an incorret format! {t}");

                    if (!int.TryParse(t.Type.Substring(start, end - start), out int result) && type.ToLower() != "decimal") continue;


                    if (type == "decimal") //caso especial onde traz entre parênteres (X,Y)
                    {
                        var conteudoParenteses = t.Type.Substring(start, end - start);

                        var partes = conteudoParenteses.Split(',');

                        t.SizeField = int.Parse(partes[0]);
                        t.DecimalPrecision = int.Parse(partes[0]);
                        t.Type = type;

                        continue;
                    }

                    t.SizeField = result;
                    t.Type = type;
                }
                return tb;
            }
        }
    }
}
