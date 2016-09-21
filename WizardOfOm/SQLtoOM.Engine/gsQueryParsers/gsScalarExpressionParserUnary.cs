using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserUnary : gsScalarExpressionParserBase {
        private UnaryExpression _ScalarExpression;

        public gsScalarExpressionParserUnary(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as UnaryExpression;
        }

        public override gsSelectColumn Parse() {
            gsUnaryColumn selCol = new gsUnaryColumn();
            selCol.ColumnAlias = ColumnAlias;
            selCol.Value = 1;
            selCol.UnaryType = _ScalarExpression.UnaryExpressionType.ToUnaryType();

            return selCol;
        }
    }
}
