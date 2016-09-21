using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;
using System;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserLiteral : gsScalarExpressionParserBase {
        private Literal _ScalarExpression;

        public gsScalarExpressionParserLiteral(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as Literal;
        }

        public override gsSelectColumn Parse() {
            gsSelectColumn selCol;

            if (_ScalarExpression is IntegerLiteral) {
                var intLiteral = _ScalarExpression as IntegerLiteral;
                selCol = new gsNumberColumn() {
                    Value = intLiteral.Value,
                    ColumnAlias = ColumnAlias
                };
            }
            else if (_ScalarExpression is StringLiteral) {
                var stringLiteral = _ScalarExpression as StringLiteral;

                selCol = new gsStringColumn() {
                    Value = stringLiteral.Value,
                    ColumnAlias = ColumnAlias
                };

                if (IsDate(Convert.ToString(stringLiteral.Value))) {
                    selCol = new gsDateColumn();
                    selCol.ColumnName = "@sqlPrm" + SqlParameters.NewID();
                    selCol.Value = $"DateTime.Parse({stringLiteral.Value.Quoted()})";

                    AddParameter(selCol, false);
                }
            }
            else if (_ScalarExpression is NullLiteral) {
                var intLiteral = _ScalarExpression as NullLiteral;
                selCol = new gsNullColumn() {
                    Value = intLiteral.Value,
                    ColumnAlias = ColumnAlias
                };
            }
            else {
                throw new NotImplementedException($"Literal of type {_ScalarExpression.GetType().Name} not supported");
            }

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
