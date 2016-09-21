using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.gsQueryParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtoOM.Engine.Factories {
    internal static class gsScalarExpressionParserFactory {

        internal static gsScalarExpressionParserBase CreateParser(ScalarExpression expression, string columnAlias) {
            if (expression is ColumnReferenceExpression) {
                return new gsScalarExpressionParserColumnReference(expression, columnAlias);
            }
            else if (expression is Literal) {
                return new gsScalarExpressionParserLiteral(expression, columnAlias);
            }
            else if (expression is SearchedCaseExpression) {
                return new gsScalarExpressionParserSearchedCase(expression, columnAlias);
            }
            else if (expression is VariableReference) {
                return new gsScalarExpressionParserVariableReference(expression, columnAlias);
            }
            else if (expression is FunctionCall) {
                return new gsScalarExpressionParserFunctionCall(expression, columnAlias);
            }
            else if (expression is ScalarSubquery) {
                return new gsScalarExpressionParserScalarSubquery(expression, columnAlias);
            }
            else if (expression is CoalesceExpression) {
                return new gsScalarExpressionParserCoalesce(expression, columnAlias);
            }
            else if (expression is ParenthesisExpression) {
                return new gsScalarExpressionParserParenthesis(expression, columnAlias);
            }
            else if (expression is BinaryExpression) {
                return new gsScalarExpressionParserBinary(expression, columnAlias);
            }
            else if (expression is UnaryExpression) {
                return new gsScalarExpressionParserUnary(expression, columnAlias);
            }
            else {
                throw new NotImplementedException($"ScalarExpression of type {expression.GetType().Name} not supported");
            }
        }
    }
}
