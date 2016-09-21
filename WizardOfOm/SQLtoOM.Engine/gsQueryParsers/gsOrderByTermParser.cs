using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Factories;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {

    internal class gsOrderByTermParser {

        internal gsOrderByTerm GetOrderByTerm(ExpressionWithSortOrder orderByElement) {
            gsOrderByTerm orderByTerm;

            gsSelectColumn selCol = gsScalarExpressionParserFactory.CreateParser(orderByElement.Expression, null).Parse();
            orderByTerm = new gsOrderByTerm() {
                Direction = orderByElement.SortOrder.ToOrderByDirection(),
                Field = selCol.ColumnName
            };

            orderByTerm.Table = selCol.Table;

            return orderByTerm;
        }
    }
}
