using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal abstract class gsScalarExpressionParserBase {
        public string ColumnAlias { get; set; }

        public gsScalarExpressionParserBase(ScalarExpression expression, string columnAlias) {
            ColumnAlias = columnAlias;
        }

        public abstract gsSelectColumn Parse();
    }
}
