using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {

    internal class gsJoinClauseParser {

        public List<gsJoin> Joins = new List<gsJoin>();

        internal void GetJoins(QualifiedJoin qualifiedJoin) {
            gsJoin join = new gsJoin();
            join.JoinType = qualifiedJoin.QualifiedJoinType.ToJoinType();

            if (qualifiedJoin.FirstTableReference is QualifiedJoin) {
                GetJoins(qualifiedJoin.FirstTableReference as QualifiedJoin);
            }
            else if (qualifiedJoin.FirstTableReference is NamedTableReference) {
                join.TableA = GetTable(qualifiedJoin.FirstTableReference as NamedTableReference);
            }
            else if (qualifiedJoin.FirstTableReference is QueryDerivedTable) {
                join.TableA = GetTable(qualifiedJoin.FirstTableReference as QueryDerivedTable);
            }
            else {
                throw new NotImplementedException($"Join table reference of type {qualifiedJoin.FirstTableReference.GetType().Name} not supported");
            }

            if (qualifiedJoin.SecondTableReference is QualifiedJoin) {
                GetJoins(qualifiedJoin.SecondTableReference as QualifiedJoin);
            }
            else if (qualifiedJoin.SecondTableReference is NamedTableReference) {
                join.TableB = GetTable(qualifiedJoin.SecondTableReference as NamedTableReference);
            }
            else if (qualifiedJoin.SecondTableReference is QueryDerivedTable) {
                join.TableB = GetTable(qualifiedJoin.SecondTableReference as QueryDerivedTable);
            }
            else {
                throw new NotImplementedException($"Join table reference of type {qualifiedJoin.SecondTableReference.GetType().Name} not supported");
            }

            join.JoinClause = new gsWhereClauseParser().GetWhereClause(qualifiedJoin.SearchCondition);

            // SqlOm Join syntax is problematic when using the overload with WhereClause. 
            // Left table is not needed, so we pass the last right table just to be valid for compilation.
            try {
                if (join.TableA == null) {
                    join.TableA = Joins.Last().TableB;
                }
                if (join.TableB == null) {
                    join.TableB = Joins.Last().TableB;
                }
            }
            catch {
                throw new NotImplementedException("Join.JoinClause.Terms is only supported as WhereTermCompare");
            }

            Joins.Add(join);
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
