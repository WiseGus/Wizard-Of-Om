using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Factories;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserCoalesce : gsScalarExpressionParserBase {
        private CoalesceExpression _ScalarExpression;

        public gsScalarExpressionParserCoalesce(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as CoalesceExpression;
        }

        public override gsSelectColumn Parse() {
            gsCoalesceColumn selCol = new gsCoalesceColumn();
            selCol.ColumnAlias = ColumnAlias;

            foreach (var expr in _ScalarExpression.Expressions) {
                var parser = gsScalarExpressionParserFactory.CreateParser(expr, null);
                selCol.Columns.Add(parser.Parse());
            }

            return selCol;
        }
    }
}
