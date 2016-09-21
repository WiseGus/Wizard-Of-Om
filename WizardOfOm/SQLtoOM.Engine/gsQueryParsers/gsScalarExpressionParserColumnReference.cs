using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;
using System;
using System.Linq;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsScalarExpressionParserColumnReference : gsScalarExpressionParserBase {
        private ColumnReferenceExpression _ScalarExpression;
        
        public gsScalarExpressionParserColumnReference(ScalarExpression expression, string columnAlias)
            : base(expression, columnAlias) {
            _ScalarExpression = expression as ColumnReferenceExpression;
        }

        public override gsSelectColumn Parse() {
            gsSelectColumn selCol;

            switch (_ScalarExpression.ColumnType) {
                case ColumnType.Regular:
                    if (_ScalarExpression.MultiPartIdentifier.Identifiers.Count == 1) {
                        string columnName = _ScalarExpression.MultiPartIdentifier.Identifiers.First().Value;
                        selCol = new gsFieldColumn() {
                            ColumnName = columnName,
                            ColumnAlias = ColumnAlias
                        };
                    }
                    else if (_ScalarExpression.MultiPartIdentifier.Identifiers.Count > 1) {
                        string tableName = _ScalarExpression.MultiPartIdentifier.Identifiers[0].Value;
                        string columnName = _ScalarExpression.MultiPartIdentifier.Identifiers[1].Value;
                        selCol = new gsFieldColumn() {
                            ColumnName = columnName,
                            ColumnAlias = ColumnAlias,
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
                        ColumnAlias = ColumnAlias,
                    };
                    break;
                default:
                    throw new NotImplementedException($"ColumnReferenceExpression {_ScalarExpression.ColumnType.ToString()} not supported");
            }

            return selCol;
        }
    }
}
