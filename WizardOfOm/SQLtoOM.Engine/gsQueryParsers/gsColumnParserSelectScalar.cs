using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Factories;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsColumnParserSelectScalar : gsColumnParserBase {
        private SelectScalarExpression _Expression;

        public gsColumnParserSelectScalar(SelectElement element)
            : base(element) {
            _Expression = element as SelectScalarExpression;
        }

        public override gsSelectColumn Parse() {
            gsSelectColumn selCol;

            string columnAlias = _Expression.ColumnName != null ? _Expression.ColumnName.Value : null;

            ScalarExpression expression = _Expression.Expression;
            gsScalarExpressionParserBase parser = gsScalarExpressionParserFactory.CreateParser(expression, columnAlias);
            selCol = parser.Parse();

            return selCol;
        }
    }
}
