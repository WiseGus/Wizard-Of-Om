using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.gsQueryParsers;
using System;

namespace SQLtoOM.Engine.Factories {
    internal static class gsColumnParserFactory {

        internal static gsColumnParserBase CreateParser(SelectElement selectElement) {
            if (selectElement is SelectStarExpression) {
                return new gsColumnParserSelectStar(selectElement);
            }
            else if (selectElement is SelectScalarExpression) {
                return new gsColumnParserSelectScalar(selectElement);
            }
            else {
                throw new NotImplementedException($"Select of type {selectElement.GetType().Name} not supported");
            }
        }
    }
}
