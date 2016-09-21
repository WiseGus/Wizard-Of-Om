using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;
using System.Collections.Generic;
using SQLtoOM.Engine.Factories;

namespace SQLtoOM.Engine.gsQueryParsers {

    internal class gsWhereClauseParser {

        internal gsWhereClause GetWhereClause(BooleanExpression searchCondition) {
            gsWhereClause whereClause = new gsWhereClause();

            GetWhereClause(searchCondition, whereClause);

            return whereClause;
        }

        internal void GetWhereClause(BooleanExpression searchCondition, gsWhereClause whereClause) {
            if (searchCondition is BooleanComparisonExpression) {
                gsWhereTermBase whereTerm = GetWhereTerm(searchCondition as BooleanComparisonExpression);
                whereClause.Terms.Add(whereTerm);
            }
            else if (searchCondition is BooleanParenthesisExpression) {
                var booleanParenthesisExpression = searchCondition as BooleanParenthesisExpression;
                gsWhereClause subClause = GetWhereClause(booleanParenthesisExpression.Expression);
                whereClause.SubClauses.Add(subClause);
            }
            else if (searchCondition is BooleanIsNullExpression) {
                gsWhereTermBase whereTerm = GetWhereTerm(searchCondition as BooleanIsNullExpression);
                whereClause.Terms.Add(whereTerm);
            }
            else if (searchCondition is BooleanBinaryExpression) {
                var booleanBinaryExpression = searchCondition as BooleanBinaryExpression;

                whereClause.Relationship = booleanBinaryExpression.BinaryExpressionType == BooleanBinaryExpressionType.And ? gsWhereClauseRelationship.And : gsWhereClauseRelationship.Or;

                if (booleanBinaryExpression.FirstExpression.IsBinaryExpression()) {
                    gsWhereClause subClause = new gsWhereClause();
                    whereClause.SubClauses.Add(subClause);
                    GetWhereClause(booleanBinaryExpression.FirstExpression, subClause);
                }
                else {
                    GetWhereClause(booleanBinaryExpression.FirstExpression, whereClause);
                }

                if (booleanBinaryExpression.SecondExpression.IsBinaryExpression()) {
                    gsWhereClause subClause = new gsWhereClause();
                    whereClause.SubClauses.Add(subClause);
                    GetWhereClause(booleanBinaryExpression.SecondExpression, subClause);
                }
                else {
                    GetWhereClause(booleanBinaryExpression.SecondExpression, whereClause);
                }
            }
            else if (searchCondition is LikePredicate) {
                gsWhereTermBase whereTerm = GetWhereTerm(searchCondition as LikePredicate);
                whereClause.Terms.Add(whereTerm);
            }
            else if (searchCondition is ExistsPredicate) {
                gsWhereTermBase whereTerm = GetWhereTerm(searchCondition as ExistsPredicate, gsExistsOrNotExists.Exists);
                whereClause.Terms.Add(whereTerm);
            }
            else if (searchCondition is BooleanNotExpression) {
                if ((searchCondition as BooleanNotExpression).Expression is ExistsPredicate) {
                    gsWhereTermBase whereTerm = GetWhereTerm((searchCondition as BooleanNotExpression).Expression as ExistsPredicate, gsExistsOrNotExists.NotExists);
                    whereClause.Terms.Add(whereTerm);
                }
                else {
                    throw new NotImplementedException($"BooleanNotExpression of type {(searchCondition as BooleanNotExpression).Expression.GetType().Name} not supported");
                }
            }
            else if (searchCondition is InPredicate) {
                gsWhereTermBase whereTerm = GetWhereTerm(searchCondition as InPredicate, (searchCondition as InPredicate).NotDefined);
                whereClause.Terms.Add(whereTerm);
            }
            else {
                throw new NotImplementedException($"BooleanExpression of type {searchCondition.GetType().Name} not supported");
            }
        }

        internal gsWhereTermBase GetWhereTerm(BooleanComparisonExpression booleanComparisonExpression) {
            gsWhereTermCompare whereTerm = new gsWhereTermCompare();

            whereTerm.Operator = booleanComparisonExpression.ComparisonType.ToCompareOperator();
            whereTerm.FirstExpression = gsScalarExpressionParserFactory.CreateParser(booleanComparisonExpression.FirstExpression, null).Parse();
            whereTerm.SecondExpression = gsScalarExpressionParserFactory.CreateParser(booleanComparisonExpression.SecondExpression, null).Parse();

            return whereTerm;
        }

        internal gsWhereTermBase GetWhereTerm(BooleanIsNullExpression booleanIsNullExpression) {
            gsWhereTermIsNullOrNotNull whereTerm = new gsWhereTermIsNullOrNotNull();

            whereTerm.NullType = booleanIsNullExpression.IsNot ? gsNullOrNotNull.NotNull : gsNullOrNotNull.Null;
            whereTerm.Expression = gsScalarExpressionParserFactory.CreateParser(booleanIsNullExpression.Expression, null).Parse();

            return whereTerm;
        }

        internal gsWhereTermBase GetWhereTerm(LikePredicate likePredicate) {
            gsWhereTermCompare whereTerm = new gsWhereTermCompare();

            whereTerm.Operator = gsCompareOperator.Like;
            whereTerm.FirstExpression = gsScalarExpressionParserFactory.CreateParser(likePredicate.FirstExpression, null).Parse();
            whereTerm.SecondExpression = gsScalarExpressionParserFactory.CreateParser(likePredicate.SecondExpression, null).Parse();

            return whereTerm;
        }

        internal gsWhereTermBase GetWhereTerm(ExistsPredicate existsPredicate, gsExistsOrNotExists existsOrNotExists) {
            gsWhereTermExistsOrNotExists whereTerm = new gsWhereTermExistsOrNotExists();
            whereTerm.ExistsType = existsOrNotExists;

            if (existsPredicate.Subquery.QueryExpression is QuerySpecification) {
                gsSelectQuery subQry = new gsSelectQuery();
                subQry.QryName = $"subQry{gsSelectQuery.GetNextID()}";
                whereTerm.QueryName = subQry.QryName;

                QuerySpecification qrySpec = existsPredicate.Subquery.QueryExpression as QuerySpecification;

                gsSelectQueryParser qryParser = new gsSelectQueryParser();
                qryParser.ProcessQuerySpecification(qrySpec, subQry);

                whereTerm.Sql = subQry.ToString();
            }
            else {
                throw new NotImplementedException($"QuerySpecification {existsPredicate.Subquery.QueryExpression.GetType().Name} not supported");
            }

            return whereTerm;
        }

        internal gsWhereTermBase GetWhereTerm(InPredicate inPredicate, bool isNotIn) {
            gsWhereTermIn whereTerm = new gsWhereTermIn();
            string subQryName = $"subQry{gsSelectQuery.GetNextID()}";

            whereTerm.InType = isNotIn ? gsInOrNotIn.NotIn : gsInOrNotIn.In;

            whereTerm.Expression = gsScalarExpressionParserFactory.CreateParser(inPredicate.Expression, null).Parse();

            if (inPredicate.Subquery != null) {
                gsSelectQuery subQry = new gsSelectQuery();
                whereTerm.QueryName = subQry.QryName = subQryName;

                QuerySpecification qrySpec = inPredicate.Subquery.QueryExpression as QuerySpecification;

                gsSelectQueryParser qryParser = new gsSelectQueryParser();
                qryParser.ProcessQuerySpecification(qrySpec, subQry);

                whereTerm.Sql = subQry.ToString();
            }
            else if (inPredicate.Values.Count > 0) {
                List<string> values = new List<string>();

                foreach (ScalarExpression val in inPredicate.Values) {
                    string valueStr = Convert.ToString(gsScalarExpressionParserFactory.CreateParser(val, null).Parse().Value);
                    values.Add(valueStr);
                }

                whereTerm.values = values;
            }
            else {
                throw new NotImplementedException("InPredicate type not supported");
            }

            return whereTerm;
        }
    }
}
