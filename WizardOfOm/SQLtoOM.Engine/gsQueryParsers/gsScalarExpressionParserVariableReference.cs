using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserVariableReference : gsScalarExpressionParserBase {
        private VariableReference _ScalarExpression;

        public gsScalarExpressionParserVariableReference(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as VariableReference;
        }

        public override gsSelectColumn Parse() {
            gsSelectColumn selCol;

            selCol = new gsParameterColumn();
            selCol.ColumnName = _ScalarExpression.Name;

            AddParameter(selCol, true);

            return selCol;
        }

        private void AddParameter(gsSelectColumn column, bool addQuotes) {
            if (column is gsDateColumn || column is gsParameterColumn) {
                SqlParameters.Instance.Add(new gsParameter {
                    Name = column.ColumnName,
                    Value = column.Value == null ? "{VALUE}" : column.Value,
                    AddQuotes = addQuotes
                });
            }
        }
    }
}
