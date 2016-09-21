using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;
using System;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserScalarSubquery : gsScalarExpressionParserBase {
        private ScalarSubquery _ScalarExpression;

        public gsScalarExpressionParserScalarSubquery(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as ScalarSubquery;
        }

        public override gsSelectColumn Parse() {
            gsSubQueryColumn subQryCol = new gsSubQueryColumn();
            subQryCol.ColumnAlias = ColumnAlias;

            if (_ScalarExpression.QueryExpression is QuerySpecification) {
                gsSelectQuery subQry = new gsSelectQuery();
                QuerySpecification qrySpec = _ScalarExpression.QueryExpression as QuerySpecification;

                gsSelectQueryParser qryParser = new gsSelectQueryParser();
                qryParser.ProcessQuerySpecification(qrySpec, subQry);

                subQryCol.SubQuery = subQry;
            }
            else {
                throw new NotImplementedException($"QuerySpecification {_ScalarExpression.QueryExpression.GetType().Name} not supported");
            }

            return subQryCol;
        }
    }
}
