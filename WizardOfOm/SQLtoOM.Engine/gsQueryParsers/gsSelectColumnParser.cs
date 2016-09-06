using Microsoft.SqlServer.TransactSql.ScriptDom;
using Reeb.SqlOM;
using SQLtoOM.Engine.Models;
using System;
using System.Linq;

namespace SQLtoOM.Engine.gsQueryParsers {

    internal class gsSelectColumnParser {

        internal gsSelectColumn GetSelectColumn(SelectElement selectElement) {
            gsSelectColumn selCol;

            if (selectElement is SelectStarExpression) {
                selCol = GetSelectColumn(selectElement as SelectStarExpression);
            }
            else if (selectElement is SelectScalarExpression) {
                selCol = GetSelectColumn(selectElement as SelectScalarExpression);
            }
            else {
                throw new NotImplementedException($"Select of type {selectElement.GetType().Name} not supported");
            }

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(SelectStarExpression expression) {
            gsSelectColumn selCol;

            if (expression.Qualifier != null) {
                if (expression.Qualifier is MultiPartIdentifier) {
                    if ((expression.Qualifier as MultiPartIdentifier).Identifiers.Count == 1) {
                        string tableName = (expression.Qualifier as MultiPartIdentifier).Identifiers.First().Value;
                        selCol = new gsFieldColumn() {
                            ColumnName = "*",
                            Table = new gsFromTerm { TableName = tableName }
                        };
                    }
                    else {
                        throw new NotImplementedException("Expression.Qualifier.Identifiers.Count > 1 not supported");
                    }
                }
                else {
                    throw new NotImplementedException($"Expression.Qualifier of type {expression.Qualifier.GetType().Name} not supported");
                }
            }
            else {
                selCol = selCol = new gsFieldColumn() {
                    ColumnName = "*",
                };
            }

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(SelectScalarExpression expression) {
            gsSelectColumn selCol;

            string columnAlias = expression.ColumnName != null ? expression.ColumnName.Value : null;
            selCol = GetSelectColumn(expression.Expression, columnAlias);

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(ScalarExpression expression, string columnAlias) {
            gsSelectColumn selCol;

            if (expression is ColumnReferenceExpression) {
                selCol = GetSelectColumn(expression as ColumnReferenceExpression, columnAlias);
            }
            else if (expression is Literal) {
                selCol = GetSelectColumn(expression as Literal, columnAlias);
            }
            else if (expression is SearchedCaseExpression) {
                selCol = GetSelectColumn(expression as SearchedCaseExpression, columnAlias);
            }
            else if (expression is VariableReference) {
                selCol = GetSelectColumn(expression as VariableReference, columnAlias);
            }
            else if (expression is FunctionCall) {
                selCol = GetSelectColumn(expression as FunctionCall, columnAlias);
            }
            else if (expression is ScalarSubquery) {
                selCol = GetSelectColumn(expression as ScalarSubquery, columnAlias);
            }
            else if (expression is CoalesceExpression) {
                selCol = GetSelectColumn(expression as CoalesceExpression, columnAlias);
            }
            else if (expression is ParenthesisExpression) {
                selCol = GetSelectColumn((expression as ParenthesisExpression).Expression, columnAlias);
            }
            else if (expression is BinaryExpression) {
                selCol = GetSelectColumn((expression as BinaryExpression), columnAlias);
            }
            else if (expression is UnaryExpression) {
                selCol = GetSelectColumn((expression as UnaryExpression), columnAlias);
            }
            else {
                throw new NotImplementedException($"ScalarExpression of type {expression.GetType().Name} not supported");
            }

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(ColumnReferenceExpression expression, string columnAlias) {
            gsSelectColumn selCol;

            switch (expression.ColumnType) {
                case ColumnType.Regular:
                    if (expression.MultiPartIdentifier.Identifiers.Count == 1) {
                        string columnName = expression.MultiPartIdentifier.Identifiers.First().Value;
                        selCol = new gsFieldColumn() {
                            ColumnName = columnName,
                            ColumnAlias = columnAlias
                        };
                    }
                    else if (expression.MultiPartIdentifier.Identifiers.Count > 1) {
                        string tableName = expression.MultiPartIdentifier.Identifiers[0].Value;
                        string columnName = expression.MultiPartIdentifier.Identifiers[1].Value;
                        selCol = new gsFieldColumn() {
                            ColumnName = columnName,
                            ColumnAlias = columnAlias,
                            Table = new gsFromTerm { TableName = tableName }
                        };
                    }
                    else {
                        throw new NotImplementedException("MultiPartIdentifier.Identifiers > 2 not supported");
                    }
                    break;
                case ColumnType.Wildcard:
                    selCol = new gsFieldColumn() {
                        ColumnName = "*",
                        ColumnAlias = columnAlias,
                    };
                    break;
                default:
                    throw new NotImplementedException($"ColumnReferenceExpression {expression.ColumnType.ToString()} not supported");
            }

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(Literal expression, string columnAlias) {
            gsSelectColumn selCol;

            if (expression is IntegerLiteral) {
                var intLiteral = expression as IntegerLiteral;
                selCol = new gsNumberColumn() {
                    Value = intLiteral.Value,
                    ColumnAlias = columnAlias
                };
            }
            else if (expression is StringLiteral) {
                var stringLiteral = expression as StringLiteral;

                selCol = new gsStringColumn() {
                    Value = stringLiteral.Value,
                    ColumnAlias = columnAlias
                };

                if (IsDate(Convert.ToString(stringLiteral.Value))) {
                    selCol = new gsDateColumn();
                    selCol.ColumnName = "@sqlPrm" + SqlParameters.NewID();
                    selCol.Value = $"DateTime.Parse({stringLiteral.Value.Quoted()})";

                    AddParameter(selCol, false);
                }
            }
            else if (expression is NullLiteral) {
                var intLiteral = expression as NullLiteral;
                selCol = new gsNullColumn() {
                    Value = intLiteral.Value,
                    ColumnAlias = columnAlias
                };
            }
            else {
                throw new NotImplementedException($"Literal of type {expression.GetType().Name} not supported");
            }

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(SearchedCaseExpression expression, string columnAlias) {
            gsCaseColumn caseCol = new gsCaseColumn();
            caseCol.ColumnAlias = columnAlias;

            foreach (SearchedWhenClause whenClause in expression.WhenClauses) {
                gsCaseTerm caseTerm = new gsCaseTerm();

                var when = new gsWhereClauseParser().GetWhereClause(whenClause.WhenExpression);

                if (when.SubClauses.Count > 0) {
                    throw new NotImplementedException("Case column with subclauses not supported");
                }

                caseTerm.When = when;
                caseTerm.Then = new gsSelectColumnParser().GetSelectColumn(whenClause.ThenExpression, null);
                caseCol.CaseTerms.Add(caseTerm);
            }

            if (expression.ElseExpression != null) {
                caseCol.Else = new gsSelectColumnParser().GetSelectColumn(expression.ElseExpression, columnAlias);
            }

            return caseCol;
        }

        internal gsSelectColumn GetSelectColumn(VariableReference expression, string columnAlias) {
            gsSelectColumn selCol;

            selCol = new gsParameterColumn();
            selCol.ColumnName = expression.Name;

            AddParameter(selCol, true);

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(FunctionCall expression, string columnAlias) {
            gsFunctionColumn selCol = new gsFunctionColumn();

            if (expression.Parameters.Count > 1) {
                throw new NotImplementedException("Function parameters support only 1 value");
            }

            ScalarExpression param = expression.Parameters.First();
            var tmpCol = GetSelectColumn(param, columnAlias);

            selCol.ColumnName = tmpCol.ColumnName;
            selCol.ColumnAlias = tmpCol.ColumnAlias;
            selCol.Table = tmpCol.Table;
            selCol.Value = tmpCol.Value;
            selCol.ToStringUseExpression = tmpCol.ToStringUseExpression;

            selCol.Function = expression.FunctionName.GetFunctionName();

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(ScalarSubquery expression, string columnAlias) {
            gsSubQueryColumn subQryCol = new gsSubQueryColumn();
            subQryCol.ColumnAlias = columnAlias;

            if (expression.QueryExpression is QuerySpecification) {
                gsSelectQuery subQry = new gsSelectQuery();
                QuerySpecification qrySpec = expression.QueryExpression as QuerySpecification;

                gsSelectQueryParser qryParser = new gsSelectQueryParser();
                qryParser.ProcessQuerySpecification(qrySpec, subQry);

                subQryCol.SubQuery = subQry;
            }
            else {
                throw new NotImplementedException($"QuerySpecification {expression.QueryExpression.GetType().Name} not supported");
            }

            return subQryCol;
        }

        internal gsSelectColumn GetSelectColumn(CoalesceExpression expression, string columnAlias) {
            gsCoalesceColumn selCol = new gsCoalesceColumn();
            selCol.ColumnAlias = columnAlias;

            gsSelectColumnParser colParser = new gsSelectColumnParser(); 

            foreach (var expr in expression.Expressions) {
                selCol.Columns.Add(colParser.GetSelectColumn(expr, null));
            }

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(BinaryExpression expression, string columnAlias) {
            gsBinaryColumn selCol = new gsBinaryColumn();
            selCol.ColumnAlias = columnAlias;

            selCol.OperationType = expression.BinaryExpressionType.ToMathOperationType();
            selCol.ColumnA = GetSelectColumn(expression.FirstExpression, null);
            selCol.ColumnB = GetSelectColumn(expression.SecondExpression, null);

            return selCol;
        }

        internal gsSelectColumn GetSelectColumn(UnaryExpression expression, string columnAlias) {
            gsUnaryColumn selCol = new gsUnaryColumn();
            selCol.ColumnAlias = columnAlias;
            selCol.Value = 1;
            selCol.UnaryType = expression.UnaryExpressionType.ToUnaryType();

            return selCol;
        }
        

        private bool IsDate(string value) {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }

        private void AddParameter(gsSelectColumn column, bool addQuotes) {
            if (column is gsDateColumn || column is gsParameterColumn) {
                SqlParameters.Instance.Add(new gsParameter {
                    Name = column.ColumnName,
                    Value = column.Value == null ? "{VALUE}" : column.Value,
                    AddQuotes = addQuotes
                });
            }
        }
    }
}
