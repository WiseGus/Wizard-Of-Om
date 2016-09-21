using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;
using System;
using System.Linq;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal class gsColumnParserSelectStar : gsColumnParserBase {
        private SelectStarExpression _Expression;

        public gsColumnParserSelectStar(SelectElement element)
            : base(element) {
            _Expression = element as SelectStarExpression;
        }

        public override gsSelectColumn Parse() {
            gsSelectColumn selCol;

            if (_Expression.Qualifier != null) {
                if (_Expression.Qualifier is MultiPartIdentifier) {
                    if ((_Expression.Qualifier as MultiPartIdentifier).Identifiers.Count == 1) {
                        string tableName = (_Expression.Qualifier as MultiPartIdentifier).Identifiers.First().Value;
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
                    throw new NotImplementedException($"Expression.Qualifier of type {_Expression.Qualifier.GetType().Name} not supported");
                }
            }
            else {
                selCol = selCol = new gsFieldColumn() {
                    ColumnName = "*",
                };
            }

            return selCol;
        }
    }
}
