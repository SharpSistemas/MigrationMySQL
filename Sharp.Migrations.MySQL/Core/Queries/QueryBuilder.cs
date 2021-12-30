using Sharp.MySQL.Migrations.Core.Models;
using Sharp.MySQL.Migrations.Exceptions;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System;

namespace Sharp.MySQL.Migrations.Core.Queries
{
    internal class QueryBuilder
    {
        public static string buildQueryCreateTable(TableMapper table)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"CREATE TABLE {table.TableName} ( ");

            string idxPrimaryKey = "";
            var idxsUnique = new List<string>();

            foreach (var c in table.Columns)
            {
                sb.Append($"{c.NameField} ");
                sb.Append($"{c.TypeField} ");

                if (c.DecimalPrecision != null) //Foi definida a precisão do campo
                {
                    if (c.SizeField == null) throw new Exceptions.InvalidAttributeException($"Precision of field was defined, but its size not! {c.NameField}.");
                    sb.Append($"({c.SizeField},{c.DecimalPrecision}) ");
                }
                else //Não foi definida a precisão do campo, então eu vejo se o sizeField não é nulo. Se não for, adiciona...
                {
                    if (c.SizeField != null) sb.Append($"({c.SizeField}) ");
                }

                if (c.NotNull) sb.Append("NOT NULL ");
                else sb.Append("NULL ");

                if (c.DefaultValue != null) sb.Append($"DEFAULT {c.DefaultValue} ");

                if (c.IsUnique) idxsUnique.Add($"UNIQUE INDEX {c.NameField}_UNIQUE ({c.NameField} ASC)");
                if (c.IsAI) sb.Append("AUTO_INCREMENT ");
                if (c.IsPk) idxPrimaryKey = $"PRIMARY KEY ({c.NameField}) ";
                sb.Append(", ");
            }
            if (!string.IsNullOrEmpty(idxPrimaryKey)) sb.Append($"{idxPrimaryKey}, ");

            if (idxsUnique.Count > 0)
            {
                foreach (var i in idxsUnique) sb.Append($"{i}, ");
            }

            var queryFull = $@"{sb.ToString().Trim()
                                             .Trim(',')}) ";

            return queryFull;
        }
        public static string buildQueryAlterTable(TableMapper tbMapper, TableSchema[] colunasBD)
        {
            var colsToAdd = tbMapper.Columns.Where(col => !colunasBD.Any(o => o.Field.ToLower() == col.NameField.ToLower()))
                                            .ToArray();
            var colsToChange = diffColumnsModel_ColumnsDB(tbMapper.Columns, colunasBD);

            if (colsToAdd.Length == 0 && colsToChange.Length == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            string query = "";
            sb.Append($"ALTER TABLE {tbMapper.TableName} ");

            //função responsável por montar a parte de ADD COLUMNS
            if (colsToAdd.Length > 0) sb.Append($"{buildAddColumnsAlterTable(colsToAdd)}, ");

            //Analisa diferença entre as colunas-model e colunas-banco
            if (colsToChange.Length > 0) buildChangeColumnsAlterTable(colsToChange, colunasBD);

            query = sb.ToString().Trim()
                                 .Trim(',');

            return query;
        }
        private static string buildChangeColumnsAlterTable(Columns[] colunasModel, TableSchema[] colunasBD)
        {
            StringBuilder sb = new StringBuilder();

            string idxPrimaryKey = "";
            var idxsUnique = new List<string>();

            var colsToChange = diffColumnsModel_ColumnsDB(colunasModel, colunasBD);

            foreach (var c in colsToChange)
            {
                var colBd = colunasBD.FirstOrDefault(o => string.Equals(o.Field, c.NameField, StringComparison.InvariantCultureIgnoreCase)); //não achou nenhum campo no banco com a propriedade informada. 
                                                                                                                                             //Talvez removido/alterado no BD manualmente. Se é um campo novo na model já estará no ADD COLUMN

                if (colBd == null) continue;
                sb.Append($" CHANGE COLUMN {colBd.Field} {colBd.Field} {colBd.Type} "); //não se altera o nome nem o tipo, por isso eu uso a referência do campo no banco de dados

                if (c.DecimalPrecision != null)
                {
                    if (c.DecimalPrecision < colBd.DecimalPrecision) throw new InvalidAttributeException($"Precision defined in model is smaller than the existant in database. {c.NameField}");
                    if (c.SizeField == null) throw new InvalidAttributeException($"Precision of field was defined, but its size not! {c.NameField}.");
                    sb.Append($"({c.SizeField},{c.DecimalPrecision}) ");
                }
                else
                {
                    if (c.SizeField < colBd.SizeField) throw new InvalidAttributeException($"Size field defined in model is smaller than the existant in database {c.NameField}");
                    if (c.SizeField != null) sb.Append($"({c.SizeField}) ");
                }
                if (c.NotNull) sb.Append("NOT NULL ");
                else sb.Append("NULL ");

                if (c.DefaultValue != null) sb.Append($"DEFAULT {c.DefaultValue} ");

                if (c.IsUnique) idxsUnique.Add($"ADD UNIQUE INDEX {c.NameField}_UNIQUE ({c.NameField} ASC)");
                if (c.IsAI) sb.Append("AUTO_INCREMENT ");
                if (c.IsPk) idxPrimaryKey = $"ADD PRIMARY KEY ({c.NameField}) ";
                sb.Append(", ");
            }
            return sb.ToString().Trim(' ')
                                .Trim(',');
        }
        private static Columns[] diffColumnsModel_ColumnsDB(Columns[] colunasModel, TableSchema[] colunasBD)
        {
            if (colunasModel == null || colunasBD == null) return null;
            if (colunasModel.Length == 0 || colunasBD.Length == 0) return null;

            //vai trazer só colunas que batem o nome... se algo não bater, a coluna vai passar pelo addcolumn

            var colsToVerify = colunasModel.Where(col => colunasBD.Any(o => string.Equals(o.Field, col.NameField, StringComparison.InvariantCultureIgnoreCase)))
                                            .ToArray();

            var colsDiff = new List<Columns>();
            foreach (var c in colsToVerify)
            {
                var colBd = colunasBD.FirstOrDefault(o => string.Equals(o.Field, c.NameField, StringComparison.InvariantCultureIgnoreCase));

                if (c.DecimalPrecision > colBd.DecimalPrecision)
                {
                    colsDiff.Add(c);
                    continue;
                }
                if (c.SizeField > colBd.SizeField)
                {
                    colsDiff.Add(c);
                    continue;
                }
                if ((c.NotNull && colBd.Null == "YES") || (!c.NotNull && colBd.Null == "NO"))
                {
                    colsDiff.Add(c);
                    continue;
                }
                if (c.IsPk && colBd.Key != "PRI")
                {
                    colsDiff.Add(c);
                    continue;
                }
                if (c.IsAI && !colBd.Extra.Contains("auto_increment"))
                {
                    colsDiff.Add(c);
                    continue;
                }

                var val1 = c.DefaultValue == null ? null : c.DefaultValue.Trim('\'');
                var val2 = colBd.Default == null ? null : colBd.Default.Trim('\'');

                if (val1 == val2) continue;
                colsDiff.Add(c);
                continue;
            }
            return colsDiff.ToArray();
        }
        private static string buildAddColumnsAlterTable(Columns[] colsToAdd)
        {
            StringBuilder sb = new StringBuilder();
            string idxPrimaryKey = "";
            var idxsUnique = new List<string>();

            foreach (var c in colsToAdd)
            {
                sb.Append($"ADD COLUMN {c.NameField} ");
                sb.Append($"{c.TypeField} ");

                if (c.DecimalPrecision != null) //Foi definida a precisão do campo
                {
                    if (c.SizeField == null) throw new Exceptions.InvalidAttributeException($"Precision of field was defined, but its size not! {c.NameField}.");
                    sb.Append($"({c.SizeField},{c.DecimalPrecision}) ");
                }
                else //Não foi definida a precisão do campo, então eu vejo se o sizeField não é nulo. Se não for, adiciona...
                {
                    if (c.SizeField != null) sb.Append($"({c.SizeField}) ");
                }

                if (c.NotNull) sb.Append("NOT NULL ");
                else sb.Append("NULL ");

                if (c.DefaultValue != null) sb.Append($"DEFAULT {c.DefaultValue} ");

                if (c.IsUnique) idxsUnique.Add($"ADD UNIQUE INDEX {c.NameField}_UNIQUE ({c.NameField} ASC)");
                if (c.IsAI) sb.Append("AUTO_INCREMENT ");
                if (c.IsPk) idxPrimaryKey = $"ADD PRIMARY KEY ({c.NameField}) ";
                sb.Append(", ");
            }
            if (!string.IsNullOrEmpty(idxPrimaryKey)) sb.Append($"{idxPrimaryKey}, ");

            if (idxsUnique.Count > 0)
            {
                foreach (var i in idxsUnique) sb.Append($"{i}, ");
            }

            var queryFull = $@"{sb.ToString().Trim()
                                             .Trim(',')} ";

            return queryFull;
        }
    }
}
