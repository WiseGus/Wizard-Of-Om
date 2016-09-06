using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {

    internal class gsFromTermParser {

        internal gsFromTerm GetBaseTable(TableReference tableReference) {
            gsFromTerm table;

            if (tableReference is NamedTableReference) {
                table = GetTable(tableReference as NamedTableReference);
            }
            else if (tableReference is QualifiedJoin) {
                var qualifiedJoin = tableReference as QualifiedJoin;
                table = GetBaseTable(qualifiedJoin.FirstTableReference);
            }
            else if (tableReference is QueryDerivedTable) {
                table = GetTable(tableReference as QueryDerivedTable);
            }
            else {
                throw new NotImplementedException($"TableReference of type {tableReference.GetType().Name} not supported");
            }

            return table;
        }

        internal gsFromTerm GetTable(NamedTableReference namedTableReference) {
            gsFromTerm table;

            string tableName = namedTableReference.SchemaObject.BaseIdentifier.Value;

            if (namedTableReference.Alias != null) {
                string alias = namedTableReference.Alias.Value;
                table = new gsFromTerm() {
                    TableName = tableName,
                    TableAlias = alias
                };
            }
            else {
                table = table = new gsFromTerm() {
                    TableName = tableName
                };
            }

            return table;
        }

        internal gsFromTerm GetTable(QueryDerivedTable queryDerivedTable) {
            gsSubQueryFromTerm table = new gsSubQueryFromTerm();
            table.TableAlias = Convert.ToString(queryDerivedTable.Alias.Value);

            if (queryDerivedTable.QueryExpression is QuerySpecification) {
                gsSelectQuery subQry = new gsSelectQuery();
                QuerySpecification qrySpec = queryDerivedTable.QueryExpression as QuerySpecification;

                gsSelectQueryParser qryParser = new gsSelectQueryParser();
                qryParser.ProcessQuerySpecification(qrySpec, subQry);

                table.SubQuery = subQry;
            }
            else {
                throw new NotImplementedException($"QuerySpecification {queryDerivedTable.QueryExpression.GetType().Name} not supported");
            }

            return table;
        }
    }
}
