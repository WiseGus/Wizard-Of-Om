using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Factories;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserBinary : gsScalarExpressionParserBase {
        private BinaryExpression _ScalarExpression;

        public gsScalarExpressionParserBinary(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as BinaryExpression;
        }

        public override gsSelectColumn Parse() {
            gsBinaryColumn selCol = new gsBinaryColumn();
            selCol.ColumnAlias = ColumnAlias;

            selCol.OperationType = _ScalarExpression.BinaryExpressionType.ToMathOperationType();
            selCol.ColumnA = gsScalarExpressionParserFactory.CreateParser(_ScalarExpression.FirstExpression, null).Parse();
            selCol.ColumnB = gsScalarExpressionParserFactory.CreateParser(_ScalarExpression.SecondExpression, null).Parse();

            return selCol;
        }
    }
}
