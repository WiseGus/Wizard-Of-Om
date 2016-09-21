using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Factories;
using SQLtoOM.Engine.Models;
using System;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserSearchedCase : gsScalarExpressionParserBase {
        private SearchedCaseExpression _ScalarExpression;

        public gsScalarExpressionParserSearchedCase(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as SearchedCaseExpression;
        }

        public override gsSelectColumn Parse() {
            gsCaseColumn caseCol = new gsCaseColumn();
            caseCol.ColumnAlias = ColumnAlias;

            foreach (SearchedWhenClause whenClause in _ScalarExpression.WhenClauses) {
                gsCaseTerm caseTerm = new gsCaseTerm();

                var when = new gsWhereClauseParser().GetWhereClause(whenClause.WhenExpression);

                if (when.SubClauses.Count > 0) {
                    throw new NotImplementedException("Case column with subclauses not supported");
                }

                caseTerm.When = when;
                caseTerm.Then = gsScalarExpressionParserFactory.CreateParser(whenClause.ThenExpression, null).Parse();

                caseCol.CaseTerms.Add(caseTerm);
            }

            if (_ScalarExpression.ElseExpression != null) {
                caseCol.Else = gsScalarExpressionParserFactory.CreateParser(_ScalarExpression.ElseExpression, ColumnAlias).Parse();
            }

            return caseCol;
        }
    }
}
