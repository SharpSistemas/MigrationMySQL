using Sharp.Migrations.MySQL.Core.Models;
using Sharp.Migrations.MySQL.Exceptions;
using Sharp.Migrations.MySQL.Helpers;
using System.Linq;
using System.Text;
using static Sharp.Migrations.MySQL.Helpers.Enums;

namespace Sharp.Migrations.MySQL.Core.Queries
{
    public class BuildQ
    {
        public static string buildQueryCreateTable(TableMapper table)
        {
            string queryCreate = $"CREATE TABLE {table.TableName} (";

            var colsToAdd = table.Colunas;

            var listaQuery = new string[colsToAdd.Length];
            for (int i = 0; i < listaQuery.Length; i++)
            {
                if (colsToAdd[i].IsPk && !colsToAdd[i].IsNotNull) throw new InvalidAttributeException($"PKs fields MUST be NOT NULL! Decorate it with TypeFieldBD. Field: {colsToAdd[i].FieldName}");

                var sb = new StringBuilder();

                sb.Append($"{colsToAdd[i].FieldName} "); //nome do campo
                sb.Append($"{colsToAdd[i].TypeField} "); //tipo do campo
                if (colsToAdd[i].TypeField != TipoCampoBD.INT && colsToAdd[i].SizeField > 0) sb.Append($"({colsToAdd[i].SizeField}) "); //tamanho do campo se for >0 e se for diferente de INT
                //inserir tipo UNSIGNED
                sb.Append($"{(colsToAdd[i].IsNotNull ? "NOT NULL" : "NULL")}");
                if (colsToAdd[i].IsAI) sb.Append("AUTO_INCREMENT ");

                if (colsToAdd[i].IsPk) sb.Append($", PRIMARY KEY ({colsToAdd[i].FieldName}) ");
                if (colsToAdd[i].IsUnique) sb.Append($", UNIQUE INDEX {colsToAdd[i].FieldName} ({colsToAdd[i].FieldName} ASC) VISIBLE");

                listaQuery[i] = sb.ToString();
            }

            var queryFull = $"{queryCreate} {string.Join(',', listaQuery)})";
            return queryFull;
        }
        public static string buildQueryAlterTable(TableMapper tbMapper)
        {
            var colunasBD = Helpers.getTableSchema(tbMapper.TableName);
            //var indexDB = Helpers.getTableIndexes(tbMapper.TableName);

            //if (!needChanges(tbMapper, colunasBD, indexDB)) return string.Empty;

            var colsToAdd = tbMapper.Colunas.Where(col => !colunasBD.Any(o => o.Field == col.FieldName))
                                            .ToArray();

            StringBuilder sb = new StringBuilder();
            string query = "";
            sb.Append($"ALTER TABLE {tbMapper.TableName} ");

            //função responsável por montar a parte de ADD COLUMNS
            if (colsToAdd.Length > 0)
            {
                sb.Append(buildAddColumnsAlterTable(colsToAdd));
                sb.Append(',');
            }

            //função responsável por montar a parte onde vai realizar alterações nas colunas
            sb.Append(buildChangeColumnsAlterTable(tbMapper.Colunas, colunasBD));

            query = sb.ToString();

            return query;
        }
        private static string buildChangeColumnsAlterTable(Colunas[] colunasModel, TableSchema[] colunasBD)
        {
            StringBuilder sb = new StringBuilder();
            var tableHasPrimaryKey = false;
            tableHasPrimaryKey = colunasBD.Any(o => o.Key == "PRI"); //uma das colunas já tem primary key

            foreach (var c in colunasModel)
            {
                var colBd = colunasBD.FirstOrDefault(o => o.Field == c.FieldName); //não achou nenhum campo no banco com a propriedade informada. 
                                                                                   //Talvez removido/alterado no BD manualmente. Se é um campo novo na model já estará no ADD COLUMN

                if (colBd == null) continue;
                sb.Append($" CHANGE COLUMN {colBd.Field} {colBd.Field} {colBd.Type} ");
                
                sb.Append($" ({(c.SizeField > colBd.SizeField ? c.SizeField : colBd.SizeField)}) ");

                if (c.DefaultValue != null) sb.Append($" DEFAULT {c.DefaultValue} ");
                sb.Append($"{(c.IsNotNull ? " NOT NULL" : " NULL")}");
                if (c.IsAI && c.TypeField == TipoCampoBD.INT && c.IsNotNull && !colBd.Extra.Contains("auto_increment")) sb.Append(" AUTO_INCREMENT, ");

                if (c.IsUnique && colBd.Key == "") sb.Append($" ADD UNIQUE INDEX {colBd.Field}_UNIQUE ({colBd.Field} ASC) VISIBLE,");
                if (c.IsPk && colBd.Key == "" && (!tableHasPrimaryKey)) sb.Append($" ADD PRIMARY KEY ({c.FieldName}) ");
                sb.Append(',');
            }
            return sb.ToString().Trim(',');
        }
        private static string buildAddColumnsAlterTable(Colunas[] colsToAdd)
        {
            var sb = new StringBuilder();
            foreach (var c in colsToAdd)
            {
                sb.Append($"ADD COLUMN {c.FieldName} ");
                sb.Append($"{c.TypeField} ");
                if (c.TypeField != TipoCampoBD.INT && c.SizeField > 0) sb.Append($"({c.SizeField}) "); //tamanho do campo se for >0 e se for diferente de INT
                sb.Append($"{(c.IsNotNull ? "NOT NULL " : "NULL ")}");
                if (c.IsAI) sb.Append("AUTO_INCREMENT ");

                if (c.IsPk) sb.Append($", PRIMARY KEY ({c.FieldName}) ");
                if (c.IsUnique) sb.Append($", UNIQUE INDEX {c.FieldName} ({c.FieldName} ASC) VISIBLE");
            }
            var queryAdd = sb.ToString();
            return queryAdd;
        }
    }
}
