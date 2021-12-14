using Sharp.MySQL.Migrations.Core.Models;
using Sharp.MySQL.Migrations.Attributes;
using Sharp.MySQL.Migrations.Exceptions;
using System.Linq;
using System.Text;

namespace Sharp.MySQL.Migrations.Core.Queries
{
    internal class QueryBuilder
    {
        public static string buildQueryCreateTable(TableMapper table)
        {
            string queryCreate = $"CREATE TABLE {table.TableName} (";

            var colsToAdd = table.Columns;

            var queryList = new string[colsToAdd.Length];
            for (int i = 0; i < queryList.Length; i++)
            {
                if (colsToAdd[i].IsPk && !colsToAdd[i].IsNotNull) throw new InvalidAttributeException($"PKs fields MUST be NOT NULL! Decorate it with TypeFieldBD. Field: {colsToAdd[i].FieldName}");

                var sb = new StringBuilder();

                sb.Append($"{colsToAdd[i].FieldName} "); //field name
                sb.Append($"{colsToAdd[i].TypeField} "); //field type
                if (colsToAdd[i].TypeField == TypeField.DECIMAL)
                {
                    int size = colsToAdd[i].SizeField == 0 ? 12 : colsToAdd[i].SizeField;
                    int precision = colsToAdd[i].DecimalPrecision == 0 ? 3 : colsToAdd[i].DecimalPrecision;

                    sb.Append($" ({size},{precision}) ");
                }

                if (colsToAdd[i].TypeField != TypeField.INT &&
                    colsToAdd[i].TypeField != TypeField.DECIMAL &&
                    colsToAdd[i].SizeField > 0)
                    sb.Append($"({colsToAdd[i].SizeField}) "); //tamanho do campo se for >0 e se for diferente de INT e DECIMAL

                //inserir tipo UNSIGNED
                sb.Append($"{(colsToAdd[i].IsNotNull ? " NOT NULL " : " NULL ")}");

                if (colsToAdd[i].DefaultValue != null) sb.Append($" DEFAULT {colsToAdd[i].DefaultValue} ");

                if (colsToAdd[i].IsAI) sb.Append(" AUTO_INCREMENT ");

                if (colsToAdd[i].IsPk) sb.Append($", PRIMARY KEY ({colsToAdd[i].FieldName}) ");
                if (colsToAdd[i].IsUnique) sb.Append($", UNIQUE INDEX {colsToAdd[i].FieldName} ({colsToAdd[i].FieldName} ASC) VISIBLE");

                queryList[i] = sb.ToString();
            }

            var queryFull = $"{queryCreate} {string.Join(',', queryList)})";
            return queryFull;
        }
        public static string buildQueryAlterTable(TableMapper tbMapper, TableSchema[] colunasBD)
        {
            //var indexDB = Helpers.getTableIndexes(tbMapper.TableName);

            //if (!needChanges(tbMapper, colunasBD, indexDB)) return string.Empty;

            var colsToAdd = tbMapper.Columns.Where(col => !colunasBD.Any(o => o.Field.ToLower() == col.FieldName.ToLower()))
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
            sb.Append(buildChangeColumnsAlterTable(tbMapper.Columns, colunasBD));

            query = sb.ToString();

            return query;
        }
        private static string buildChangeColumnsAlterTable(Columns[] colunasModel, TableSchema[] colunasBD)
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
                
                if (c.DefaultValue != null) sb.Append($" DEFAULT {c.DefaultValue} ");

                if (c.IsAI &&
                    c.TypeField == TypeField.INT &&
                    c.TypeField == TypeField.DECIMAL &&
                    c.IsNotNull &&
                    !colBd.Extra.Contains("auto_increment")) sb.Append(" AUTO_INCREMENT ");

                if (c.IsUnique && colBd.Key == "") sb.Append($" ADD UNIQUE INDEX {colBd.Field}_UNIQUE ({colBd.Field} ASC) VISIBLE,");
                if (c.IsPk && colBd.Key == "" && (!tableHasPrimaryKey)) sb.Append($" ADD PRIMARY KEY ({c.FieldName}) ");
                sb.Append(',');
            }
            return sb.ToString().Trim(',');
        }
        private static string buildAddColumnsAlterTable(Columns[] colsToAdd)
        {
            var sb = new StringBuilder();
            foreach (var c in colsToAdd)
            {
                sb.Append($"ADD COLUMN {c.FieldName} ");
                sb.Append($"{c.TypeField} ");

                if (c.TypeField == TypeField.DECIMAL)
                {
                    int size = c.SizeField == 0 ? 12 : c.SizeField;
                    int precision = c.DecimalPrecision == 0 ? 3 : c.DecimalPrecision;

                    sb.Append($" ({size},{precision}) ");
                }

                if (c.TypeField != TypeField.INT &&
                    c.TypeField != TypeField.DECIMAL &&
                    c.SizeField > 0) sb.Append($"({c.SizeField}) "); //tamanho do campo se for >0 e se for diferente de INT
                sb.Append($"{(c.IsNotNull ? "NOT NULL " : "NULL ")}");

                if (c.DefaultValue != null) sb.Append($" DEFAULT {c.DefaultValue} ");

                if (c.IsAI) sb.Append("AUTO_INCREMENT ");

                if (c.IsPk) sb.Append($", PRIMARY KEY ({c.FieldName}) ");
                if (c.IsUnique) sb.Append($", UNIQUE INDEX {c.FieldName} ({c.FieldName} ASC) VISIBLE");
            }
            var queryAdd = sb.ToString();
            return queryAdd;
        }
    }
}
