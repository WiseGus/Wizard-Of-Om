using System;
using System.Collections.Generic;

namespace SQLtoOM.Engine.Models {
    internal class gsCoalesceColumn : gsSelectColumn {

        public List<gsSelectColumn> Columns { get; set; }

        public gsCoalesceColumn() {
            Columns = new List<gsSelectColumn>();
        }

        public override string ToString() {
            string columnSqlOm;

            // Always use Coalesce
            //if (Columns.Count > 2) {
            string[] columnValues = new string[Columns.Count];

            for (int i = 0; i < Columns.Count; i++) {
                if (Columns[i] is gsFieldColumn) {
                    if (Columns[i].Table != null) {
                        columnValues[i] = string.Concat(Columns[i].Table.TableName, ".", Columns[i].ColumnName);
                    }
                    else {
                        columnValues[i] = Columns[i].ColumnName;
                    }
                }
                else if (Columns[i] is gsNumberColumn) {
                    columnValues[i] = Convert.ToString(Columns[i].Value);
                }
                else if (Columns[i] is gsStringColumn) {
                    columnValues[i] = string.Concat("'", Columns[i].Value, "'");
                }
                else {
                    throw new NotImplementedException($"Coalesce {Columns[i].GetType().Name} not supported");
                }
            }

            columnSqlOm = $"SqlExpression.Raw(\"COALESCE({(string.Join(", ", columnValues))})\"), {ColumnAlias.Quoted()}";
            //}
            //else {
            //    Columns.ForEach(col => col.ToStringUseExpression = true);
            //    columnSqlOm = $"SqlExpression.IfNull({Columns[0].ToString()}, {Columns[1].ToString()}), {ColumnAlias.Quoted()}";
            //}

            return columnSqlOm;
        }
    }
}
