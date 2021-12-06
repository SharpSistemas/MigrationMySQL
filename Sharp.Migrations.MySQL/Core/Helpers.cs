using Dapper;
using Sharp.Migrations.MySQL.Core.Models;
using Sharp.Migrations.MySQL.Helpers;
using System.Linq;
using System.Text;
using static Sharp.Migrations.MySQL.Helpers.Enums;
using static Sharp.Migrations.MySQL.Helpers.TableMapper;

namespace Sharp.Migrations.MySQL.Core
{
    public static class Helpers
    {
        public static TableResult CriaModificaTabela<T>()
        {
            var tableMapper = FromType<T>();

            if (!existeTabela(tableMapper.TableName)) return DBManipulation.CriaTabela(tableMapper);
            return DBManipulation.ModificaTabela(tableMapper);
        }

        public static string[] leAtributos(Colunas[] colsToAdd)
        {
            var listaQuery = new string[colsToAdd.Length];
            for (int i = 0; i < listaQuery.Length; i++)
            {
                var sb = new StringBuilder();

                sb.Append($"{colsToAdd[i].FieldName} ");
                sb.Append($"{colsToAdd[i].TypeField} ");
                if (colsToAdd[i].TypeField != TipoCampoBD.INT && colsToAdd[i].SizeField > 0) sb.Append($"({colsToAdd[i].SizeField}) ");
                if (colsToAdd[i].IsPk) sb.Append("PRIMARY KEY ");
                if (colsToAdd[i].IsAI) sb.Append("AUTO_INCREMENT ");
                if (colsToAdd[i].IsUnique) sb.Append("UNIQUE ");
                if (colsToAdd[i].IsNotNull) sb.Append("NOT NULL ");

                listaQuery[i] = sb.ToString();
            }
            return listaQuery;
        }
        public static TableIndex[] getTableIndexes(string tableName)
        {
            using (var db = DBManipulation.Connect(""))
            {
                var indexes = db.Query<TableIndex>($"show index from {tableName}", new { tableName }).ToArray();
                return indexes;
            }
        }
        public static bool needChanges(TableMapper tbMapper, TableSchema[] colunasBD, TableIndex[] indexDB)
        {
            var cols = tbMapper.Colunas.Where(col => !colunasBD.Any(o => o.Field == col.FieldName))
                                            .ToArray();

            //TODO: Acima eu verifico se existem mais colunas na model em relação ao banco. Se existe tem que criá-las no BD.
            //Existem mais 3 possibilidades a se verificar:
            //2 - Mais mais colunas no banco em relação à model. (Idealmente não fazer nada... mantém o campo lá)

            //3 - Mais índices na model em relação ao banco
            //4 - Mais índices no banco em relação à model
            //Para as 3 e 4 pensar no que fazer (como comparar os índices, se vai remover do BD, se não vai.

            if (cols.Length > 0) return true;
            return false;
        }
        public static TableSchema[] getTableSchema(string tableName)
        {
            if (!existeTabela(tableName)) return null;
            using (var db = DBManipulation.Connect(""))
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
        public static string getTableSchema2(string tableName)
        {
            if (!existeTabela(tableName)) return null;
            using (var db = DBManipulation.Connect(""))
            {
                var tb = db.QueryFirst<(string, string)>($"SHOW CREATE TABLE {tableName}");
                return tb.Item2;
            }
        }
        public static bool existeTabela(string tableName)
        {
            using (var db = DBManipulation.Connect(""))
            {
                var tb = db.QueryFirstOrDefault("show tables like @tableName", new { tableName });
                return tb != null;
            }
        }
    }
}
