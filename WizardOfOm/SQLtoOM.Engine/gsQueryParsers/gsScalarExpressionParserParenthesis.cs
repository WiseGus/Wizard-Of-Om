using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Factories;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserParenthesis : gsScalarExpressionParserBase {
        private ParenthesisExpression _ScalarExpression;

        public gsScalarExpressionParserParenthesis(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as ParenthesisExpression;
        }

        public override gsSelectColumn Parse() {
            var expr = _ScalarExpression.Expression;
            return gsScalarExpressionParserFactory.CreateParser(expr, ColumnAlias).Parse();
        }
    }
}
