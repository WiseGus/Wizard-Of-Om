using System;

namespace SQLtoOM.Engine.Models {
    internal class gsFunctionColumn : gsSelectColumn {

        public gsSqlAggregationFunction Function { get; set; }

        public override string ToString() {
            string tmpColumnName = ColumnName.Quoted();
            string fieldExpression;

            if (!tmpColumnName.HasValue()) { // A literal may have been used
                int intLiteral;
                if (int.TryParse(Convert.ToString(Value), out intLiteral)) {
                    fieldExpression = $"SqlExpression.Number({Value})";
                }
                else {
                    fieldExpression = $"SqlExpression.String({Value.ToString().Quoted()})";
                }
            }
            else {
                fieldExpression = $"SqlExpression.Field({ColumnName.Quoted()}{(Table != null ? ", " + Table.ToString() : string.Empty)})";
            }

            if (ToStringUseExpression) {
                return $"SqlExpression.Function(SqlAggregationFunction.{Function.ToString()}, {fieldExpression})";
            }
            else {
                if (!ColumnAlias.HasValue()) {
                    ColumnAlias = "col" + GetNextID();
                }

                if (Table == null) {
                    return $"SqlExpression.Function(SqlAggregationFunction.{Function.ToString()}, {fieldExpression}){(ColumnAlias.HasValue() ? ", " + ColumnAlias.Quoted() : string.Empty)}";
                }
                else {
                    return $"{ColumnName.Quoted()}{(Table != null ? ", " + Table.ToString() : string.Empty)}{(ColumnAlias.HasValue() ? ", " + ColumnAlias.Quoted() : string.Empty)}, SqlAggregationFunction.{Function.ToString()}";
                }
            }
        }
    }
}