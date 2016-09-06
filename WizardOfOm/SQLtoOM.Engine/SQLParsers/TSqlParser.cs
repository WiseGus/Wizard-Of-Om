using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using SQLtoOM.Engine.Models;
using SQLtoOM.Engine.gsQueryParsers;
using System.Text;

namespace SQLtoOM.Engine.SQLParsers {

    public class TSqlParser : ISQLParser {

        private string _GeneratedSqlOm = null;
        public string GeneratedSqlOm
        {
            get
            {
                if (!_GeneratedSqlOm.HasValue()) {
                    _GeneratedSqlOm = _gsQry.ToString();
                    _GeneratedSqlOm += AddSqlParametersText();
                }

                gsSelectQuery.ResetID();
                gsSelectColumn.ResetID();
                gsWhereClause.ResetID();
                SqlParameters.Instance.Clear();

                return _GeneratedSqlOm;
            }
        }

        private gsSelectQuery _gsQry = new gsSelectQuery();

        public void ParseSQL(string sql) {
            TSql130Parser sqlParser = new TSql130Parser(false);

            IList<ParseError> parseErrors;
            TSqlFragment result = sqlParser.Parse(new StringReader(sql), out parseErrors);
            if (parseErrors.Count > 0) {
                throw new ArgumentException(string.Join(Environment.NewLine, parseErrors.Select(p => p.Message)));
            }

            TSqlScript SqlScript = result as TSqlScript;
            if (SqlScript.Batches.Count > 1) {
                throw new NotSupportedException("Batches.Count > 1 not supported");
            }

            var batch = SqlScript.Batches.First();
            if (batch.Statements.Count > 1) {
                throw new NotSupportedException("Statements.Count > 1 not supported");
            }

            TSqlStatement statement = batch.Statements.First();

            if (statement is SelectStatement) {
                ParseSelectStatement(statement as SelectStatement);
            }
            else {
                throw new NotImplementedException($"Query statement of type {statement.GetType().Name} not supported");
            }
        }

        private void ParseSelectStatement(SelectStatement selectStatement) {
            if (selectStatement.QueryExpression is QuerySpecification) {
                QuerySpecification qrySpec = selectStatement.QueryExpression as QuerySpecification;

                gsSelectQueryParser qryParser = new gsSelectQueryParser();
                qryParser.ProcessQuerySpecification(qrySpec, _gsQry);
            }
            else {
                throw new NotImplementedException($"QuerySpecification {selectStatement.QueryExpression.GetType().Name} not supported");
            }
        }

        private string AddSqlParametersText() {
            if (SqlParameters.Instance.Count == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("//slQueryParameters sqlParams = new slQueryParameters();");
            foreach (gsParameter param in SqlParameters.Instance) {
                sb.AppendFormat("//sqlParams.Add({0}, {1});", param.Name.Quoted(), param.AddQuotes ? param.Value.ToString().Quoted() : param.Value.ToString())
                  .AppendLine();
            }

            return sb.ToString();
        }
    }
}