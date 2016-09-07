using System.Text;
using System;
using System.Collections.Generic;

namespace SQLtoOM.Engine.Models {

    internal class gsBinaryColumn : gsSelectColumn {

        public gsMathOperationType OperationType { get; set; }
        public gsSelectColumn ColumnA { get; set; }
        public gsSelectColumn ColumnB { get; set; }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            string getFinalSqlStr = GetBinarySql(this);

            if (ColumnAlias.HasValue()) {
                sb.Append($"SqlExpression.Raw(\"{getFinalSqlStr}\"), {ColumnAlias.Quoted()}");
            }
            else {
                sb.Append($"SqlExpression.Raw(\"{getFinalSqlStr}\")");
            }

            return sb.ToString();
        }

        private string GetBinarySql(gsBinaryColumn selCol) {
            StringBuilder sb = new StringBuilder();

            if (selCol.ColumnA is gsBinaryColumn) {
                sb.Append("(");
                sb.Append(GetBinarySql(selCol.ColumnA as gsBinaryColumn));
                sb.Append(")");
            }
            else {
                sb.Append(GetColumnRawValue(selCol.ColumnA));
            }

            sb.Append(" ").Append(selCol.OperationType.GetSymbol()).Append(" ");

            if (selCol.ColumnB is gsBinaryColumn) {
                sb.Append("(");
                sb.Append(GetBinarySql(selCol.ColumnA as gsBinaryColumn));
                sb.Append(")");
            }
            else {
                sb.Append(GetColumnRawValue(selCol.ColumnB));
            }

            return sb.ToString();
        }

        private string GetColumnRawValue(gsSelectColumn column) {
            if (column is gsFieldColumn) {
                if (column.Table == null) {
                    return $"{column.ColumnName}";
                }
                else {
                    if (column.Table.TableAlias.HasValue()) {
                        return $"{column.Table.TableAlias}.{column.ColumnName}";
                    }
                    else {
                        return $"{column.Table.TableName}.{column.ColumnName}";
                    }
                }
            }
            else if (column is gsNumberColumn) {
                return Convert.ToString(column.Value);
            }
            else if (column is gsStringColumn) {
                return "'" + Convert.ToString(column.Value) + "'";
            }
            else if (column is gsUnaryColumn) {
                if ((column as gsUnaryColumn).UnaryType == gsUnaryType.Negative) {
                    return "-" + Convert.ToString(column.Value);
                }
                return Convert.ToString(column.Value);
            }
            else if (column is gsRawColumn || column is gsBinaryColumn || column is gsCoalesceColumn) {
                string rawSql = column.ToString();
                rawSql = rawSql.Replace("SqlExpression.Raw(\"", "");
                rawSql = rawSql.Remove(rawSql.Length - 4);
                return rawSql;
            }
            else {
                throw new NotImplementedException($"BinaryColumn {column.GetType().Name} not supported");
            }
        }
    }
}