using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Factories;
using SQLtoOM.Engine.Models;
using System;
using System.Linq;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserFunctionCall : gsScalarExpressionParserBase {
        private FunctionCall _ScalarExpression;

        public gsScalarExpressionParserFunctionCall(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as FunctionCall;
        }

        public override gsSelectColumn Parse() {
            gsFunctionColumn selCol = new gsFunctionColumn();

            if (_ScalarExpression.Parameters.Count > 1) {
                throw new NotImplementedException("Function parameters support only 1 value");
            }

            ScalarExpression param = _ScalarExpression.Parameters.First();
            var tmpCol = gsScalarExpressionParserFactory.CreateParser(param, ColumnAlias).Parse();

            selCol.ColumnName = tmpCol.ColumnName;
            selCol.ColumnAlias = tmpCol.ColumnAlias;
            selCol.Table = tmpCol.Table;
            selCol.Value = tmpCol.Value;
            selCol.ToStringUseExpression = tmpCol.ToStringUseExpression;

            selCol.Function = _ScalarExpression.FunctionName.GetFunctionName();

            return selCol;
        }
    }
}
