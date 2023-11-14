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
        private Dictionary<int, ISchemaChange> schemaVersions = new Dictionary<int, ISchemaChange>();

        /// <summary>
        /// Constructor class
        /// </summary>
        /// <param name="dbFac">Connection factory</param>
        public Migration(ConnectionFactory dbFac)
        {
            this.dbFac = dbFac;
            tables = new List<TableMapper>();
        }


        /// <summary>
        /// Function to add models to migration
        /// </summary>
        /// <typeparam name="T">Model class</typeparam>
        /// <returns>A list of table mapper</returns>
        [Obsolete]
        public Migration Add<T>() => AddModel<T>();

        /// <summary>
        /// Function to add models to migration
        /// </summary>
        /// <typeparam name="T">Model class</typeparam>
        /// <returns>A list of table mapper</returns>
        public Migration AddModel<T>()
        {
            var tableInfo = TableMapper.FromType<T>();

            if (tables.Any(t => t.TableName == tableInfo.TableName)) throw new InvalidOperationException($"Table `{tableInfo.TableName}` already added");

            tables.Add(tableInfo);
            return this;
        }

        /// <summary>
        /// Adds schemas changes to be executed in Migration
        /// </summary>
        /// <typeparam name="T">Change to be executed</typeparam>
        /// <returns>The object</returns>

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
        public MigrationResult Migrate()
        {
            using var cnn = dbFac.GetConnection();

            var allTables = cnn.Query<string>("SHOW TABLES;").ToArray();

            // Sempre cria
            this.AddModel<Migrations.Core.Models.Schema_Changes>();
            // Migrate Tables
            var result = tables.Select(t => migrateTable(cnn, allTables, t)).ToArray();
            tables.Clear();

            var minVersionArr = cnn.Query<int>("SELECT Schema_Version as minVersion FROM Schema_Changes ORDER BY Schema_Version DESC LIMIT 1", null).ToArray();
            int minVersion, maxVersion = 0;
            if (minVersionArr.Length == 0)
            {
                // Não tem
                cnn.Execute("INSERT INTO Schema_Changes (Schema_Version) VALUES(0)", null);
                minVersion = 0;
            }
            else
            {
                minVersion = minVersionArr[0];
            }
            var versions = schemaVersions.Where(o => o.Key > minVersion)
                                         .OrderBy(o => o.Key)
                                         .Select(o => o.Value);

            foreach (var vers in versions)
            {
                // if !CanRun exception
                var action = vers.CanRun();
                if (action == Status.Abort) throw new Exception($"Schema_Change version {vers.SchemaVersion} is not allowed to run!");

                if (action == Status.Ok)
                {
                    vers.Initialize(dbFac);
                    vers.Run();
                }

                cnn.Execute(@"UPDATE Schema_Changes SET Schema_Version=@schemaVersion, Schema_Changed=@schemaDateTime",
                    new
                    {
                        schemaVersion = vers.SchemaVersion,
                        schemaDateTime = DateTime.Now
                    });
            }

            return new MigrationResult()
            {
                tables = result,
                FirstSchemaVersion = minVersion,
                LastSchemaVersion = maxVersion,
            };
        }

        private TableResult migrateTable(IDbConnection cnn, string[] allTables, TableMapper tableMapper)
        {
            if (!existeTabela(allTables, tableMapper.TableName)) return CreateTable(cnn, tableMapper);
            return ModifyTable(cnn, tableMapper);
        }

        private TableResult CreateTable(IDbConnection cnn, TableMapper tableMapper)
        {
            string query = QueryBuilder.buildQueryCreateTable(tableMapper);
            try
            {
                var result = cnn.Execute(query, cnn);
                return new TableResult
                {
                    TableName = tableMapper.TableName,
                    ColumnsAdded = tableMapper.Columns.Length,
                    WasCreated = true,
                    WasModified = false,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while executing create table. Your CREATE TABLE: {query}", ex);
            }
        }
        private TableResult ModifyTable(IDbConnection cnn, TableMapper tbMapper)
        {
            var colunasBD = getTableSchema(cnn, tbMapper.TableName);
            var query = QueryBuilder.buildQueryAlterTable(tbMapper, colunasBD);

            if (string.IsNullOrWhiteSpace(query)) return new TableResult
            {
                TableName = tbMapper.TableName,
                ColumnsAdded = 0,
                WasCreated = false,
                WasModified = false,
            };

            try
            {
                var result = cnn.Execute(query);

                return new TableResult
                {
                    TableName = tbMapper.TableName,
                    ColumnsAdded = tbMapper.Columns.Length - colunasBD.Length,
                    WasModified = true,
                    WasCreated = false,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while executing alter table. Your ALTER TABLE: {query}", ex);
                throw;
            }
        }
        private bool existeTabela(string[] allTables, string tableName)
        {
            // CaseSensitive: https://dev.mysql.com/doc/refman/8.0/en/identifier-case-sensitivity.html
            // Se for INsensitive, ele salva minúsculo no BD
            return allTables.Any(t => t.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));
        }
        private TableIndex[] getTableIndexes(IDbConnection cnn, string tableName)
        {
            var indexes = cnn.Query<TableIndex>($"show index from {tableName}", new { tableName }).ToArray();
            return indexes;
        }
        private TableSchema[] getTableSchema(IDbConnection cnn, string tableName)
        {
            var tb = cnn.Query<TableSchema>($"describe {tableName}").ToArray();

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
                    t.DecimalPrecision = int.Parse(partes[1]);
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
