using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {

    internal class gsGroupByTermParser {

        internal gsGroupByTerm GetGroupByTerm(GroupingSpecification groupingSpec) {
            gsGroupByTerm groupByTerm;

            if (groupingSpec is ExpressionGroupingSpecification) {
                groupByTerm = GetGroupByTerm(groupingSpec as ExpressionGroupingSpecification);
            }
            else {
                throw new NotImplementedException($"ExpressionGroupingSpecification.Expression {(groupingSpec as ExpressionGroupingSpecification).Expression.GetType().Name} not supported");
            }

            return groupByTerm;
        }

        internal gsGroupByTerm GetGroupByTerm(ExpressionGroupingSpecification groupingSpec) {
            gsGroupByTerm groupByTerm;

            gsSelectColumn selCol = new gsSelectColumnParser().GetSelectColumn(groupingSpec.Expression, null);
            groupByTerm = new gsGroupByTerm {
                Field = selCol.ColumnName
            };

            groupByTerm.Table = selCol.Table;

            return groupByTerm;
        }
    }
}
