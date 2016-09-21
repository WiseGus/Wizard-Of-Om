using System;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;
using SQLtoOM.Engine.Factories;

namespace SQLtoOM.Engine.gsQueryParsers {

    internal class gsSelectQueryParser {

        internal void ProcessQuerySpecification(QuerySpecification qrySpec, gsSelectQuery qry) {
            AddTop(qrySpec, qry);
            AddDistinct(qrySpec, qry);
            AddFrom(qrySpec, qry);
            AddColumns(qrySpec, qry);
            AddJoins(qrySpec, qry);
            AddWhere(qrySpec, qry);
            AddGroupBy(qrySpec, qry);
            AddOrderBy(qrySpec, qry);
            AddHaving(qrySpec, qry);
        }

        private void AddTop(QuerySpecification qrySpec, gsSelectQuery qry) {
            if (qrySpec.TopRowFilter == null) return;

            if (qrySpec.TopRowFilter.Expression is IntegerLiteral) {
                var intLiteral = qrySpec.TopRowFilter.Expression as IntegerLiteral;
                qry.Top = Convert.ToInt32(intLiteral.Value);
            }
            else {
                throw new NotImplementedException($"TopRowFilter.Expression {qrySpec.TopRowFilter.Expression.GetType().Name} not supported");
            }
        }

        private void AddDistinct(QuerySpecification qrySpec, gsSelectQuery qry) {
            if (qrySpec.UniqueRowFilter != UniqueRowFilter.Distinct) return;

            qry.Distinct = true;
        }

        private void AddColumns(QuerySpecification qrySpec, gsSelectQuery qry) {
            foreach (var selectElement in qrySpec.SelectElements) {
                gsColumnParserBase colParser = gsColumnParserFactory.CreateParser(selectElement);
                gsSelectColumn selCol = colParser.Parse();
                qry.Columns.Add(selCol);
            }
        }

        private void AddFrom(QuerySpecification specExpr, gsSelectQuery qry) {
            if (specExpr.FromClause == null) {
                throw new NotImplementedException("Must declare a base table");
            }
            if (specExpr.FromClause.TableReferences.Count > 1) {
                throw new NotImplementedException("FromClause.TableReferences.Count > 1 not supported");
            }

            var tableReference = specExpr.FromClause.TableReferences.First();
            qry.FromClause.BaseTable = new gsFromTermParser().GetBaseTable(tableReference);
        }

        private void AddJoins(QuerySpecification specExpr, gsSelectQuery qry) {
            if (specExpr.FromClause.TableReferences.Count > 1) {
                throw new NotImplementedException("Multiple table references are not supported");
            }

            var tableReference = specExpr.FromClause.TableReferences.First();
            if (!(tableReference is QualifiedJoin)) return;

            var joinsHlp = new gsJoinClauseParser();
            joinsHlp.GetJoins(tableReference as QualifiedJoin);
            qry.Joins = joinsHlp.Joins;
        }

        private void AddWhere(QuerySpecification specExpr, gsSelectQuery qry) {
            if (specExpr.WhereClause == null) return;

            BooleanExpression searchCondition = specExpr.WhereClause.SearchCondition;
            qry.WhereClause = new gsWhereClauseParser().GetWhereClause(searchCondition);
        }

        private void AddGroupBy(QuerySpecification qrySpec, gsSelectQuery qry) {
            if (qrySpec.GroupByClause == null) return;

            switch (qrySpec.GroupByClause.GroupByOption) {
                case GroupByOption.Cube:
                    qry.GroupByWithCube = true;
                    break;
                case GroupByOption.Rollup:
                    qry.GroupByWithRollup = true;
                    break;
            }

            foreach (var groupingSpec in qrySpec.GroupByClause.GroupingSpecifications) {
                gsGroupByTerm groupByTerm = new gsGroupByTermParser().GetGroupByTerm(groupingSpec);
                qry.GroupByTerms.Add(groupByTerm);
            }
        }

        private void AddOrderBy(QuerySpecification specExpr, gsSelectQuery qry) {
            if (specExpr.OrderByClause == null) return;

            foreach (ExpressionWithSortOrder orderByElement in specExpr.OrderByClause.OrderByElements) {
                gsOrderByTerm orderByTerm = new gsOrderByTermParser().GetOrderByTerm(orderByElement);
                qry.OrderByTerms.Add(orderByTerm);
            }
        }

        private void AddHaving(QuerySpecification specExpr, gsSelectQuery qry) {
            if (specExpr.HavingClause == null) return;

            BooleanExpression searchCondition = specExpr.HavingClause.SearchCondition;
            var whereClauseParser = new gsWhereClauseParser();
            qry.HavingPhrase = whereClauseParser.GetWhereClause(searchCondition);
        }
    }
}
