using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;
using System;

namespace SQLtoOM.Engine {
    internal static class Extensions {

        internal static bool HasValue(this string text) {
            return !string.IsNullOrEmpty(text);
        }

        internal static string Quoted(this string text) {
            if (!text.HasValue() || (text.StartsWith("\"") && text.EndsWith("\""))) return text;

            return "\"" + text + "\"";
        }

        internal static bool ColumnIsValid(this gsSelectColumn column) {
            if (column is gsRawColumn || column is gsBinaryColumn) return true;

            return column.ColumnName.HasValue() || Convert.ToString(column.Value).HasValue();
        }

        internal static bool IsBinaryExpression(this BooleanExpression expression) {
            return expression is BooleanBinaryExpression;
        }

        internal static string GetSymbol(this gsMathOperationType operationType) {
            switch (operationType) {
                case gsMathOperationType.Add:
                    return "+";
                case gsMathOperationType.Subtract:
                    return "-";
                case gsMathOperationType.Multiply:
                    return "*";
                case gsMathOperationType.Divide:
                    return "/";
                case gsMathOperationType.Modulo:
                    return "%";
                default:
                    return "+";
            }
        }

        internal static gsCompareOperator ToCompareOperator(this BooleanComparisonType compareType) {
            switch (compareType) {
                case BooleanComparisonType.Equals:
                    return gsCompareOperator.Equal;
                case BooleanComparisonType.GreaterThan:
                    return gsCompareOperator.Greater;
                case BooleanComparisonType.LessThan:
                    return gsCompareOperator.Less;
                case BooleanComparisonType.GreaterThanOrEqualTo:
                    return gsCompareOperator.GreaterOrEqual;
                case BooleanComparisonType.LessThanOrEqualTo:
                    return gsCompareOperator.LessOrEqual;
                case BooleanComparisonType.NotEqualToBrackets:
                case BooleanComparisonType.NotEqualToExclamation:
                    return gsCompareOperator.NotEqual;
                default:
                    throw new NotSupportedException("Unsupported CompareOperator type");
            }
        }

        internal static gsOrderByDirection ToOrderByDirection(this SortOrder sortOrder) {
            switch (sortOrder) {
                case SortOrder.Descending:
                    return gsOrderByDirection.Descending;
                default:
                    return gsOrderByDirection.Ascending;
            }
        }

        internal static gsMathOperationType ToMathOperationType(this BinaryExpressionType binaryExpressionType) {
            switch (binaryExpressionType) {
                case BinaryExpressionType.Add:
                    return gsMathOperationType.Add;
                case BinaryExpressionType.Subtract:
                    return gsMathOperationType.Subtract;
                case BinaryExpressionType.Multiply:
                    return gsMathOperationType.Multiply;
                case BinaryExpressionType.Divide:
                    return gsMathOperationType.Divide;
                case BinaryExpressionType.Modulo:
                    return gsMathOperationType.Modulo;
                default:
                    throw new NotImplementedException($"Binary expression type {binaryExpressionType.GetType().Name} not supported");
            }
        }

        internal static gsSqlAggregationFunction GetFunctionName(this Identifier functionName) {
            string function = functionName.Value;

            switch (function.ToUpper()) {
                case "SUM":
                    return gsSqlAggregationFunction.Sum;
                case "COUNT":
                    return gsSqlAggregationFunction.Count;
                case "AVG":
                    return gsSqlAggregationFunction.Avg;
                case "MIN":
                    return gsSqlAggregationFunction.Min;
                case "MAX":
                    return gsSqlAggregationFunction.Max;
                default:
                    throw new NotImplementedException($"Function {function} not supported");
            }
        }

        internal static gsUnaryType ToUnaryType(this UnaryExpressionType unaryExpressionType) {
            switch (unaryExpressionType) {
                case UnaryExpressionType.Positive:
                    return gsUnaryType.Positive;
                case UnaryExpressionType.Negative:
                    return gsUnaryType.Negative;
                default:
                    throw new NotImplementedException($"UnaryExpressionType {unaryExpressionType} not supported");
            }
        }

        internal static gsJoinType ToJoinType(this QualifiedJoinType qualifiedJoinType) {
            switch (qualifiedJoinType) {
                case QualifiedJoinType.Inner:
                    return gsJoinType.Inner;
                case QualifiedJoinType.LeftOuter:
                    return gsJoinType.Left;
                case QualifiedJoinType.RightOuter:
                    return gsJoinType.Right;
                case QualifiedJoinType.FullOuter:
                    return gsJoinType.Full;
                default:
                    return gsJoinType.Inner;
            }
        }
    }
}
