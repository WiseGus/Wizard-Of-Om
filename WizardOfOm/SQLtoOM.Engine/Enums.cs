using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtoOM.Engine {

    public enum RendererType {
        MsSQL,
        Oracle
    }

    internal enum gsWhereClauseRelationship {
        And = 0,
        Or = 1
    }

    internal enum gsNullOrNotNull {
        Null,
        NotNull
    }

    internal enum gsExistsOrNotExists {
        Exists,
        NotExists
    }

    internal enum gsUnaryType {
        Positive = 0,
        Negative = 1
    }

    internal enum gsInOrNotIn {
        In,
        NotIn
    }

    internal enum gsOrderByDirection {
        Ascending = 0,
        Descending = 1
    }

    internal enum gsMathOperationType {
        Add = 0,
        Subtract = 1,
        Multiply = 2,
        Divide = 3,
        Modulo = 4
    }

    internal enum gsJoinType {
        Inner = 0,
        Left = 1,
        Right = 2,
        Full = 3,
        Cross = 4
    }

    internal enum gsSqlAggregationFunction {
        None = 0,
        Sum = 1,
        Count = 2,
        Avg = 3,
        Min = 4,
        Max = 5,
        Grouping = 6
    }

    internal enum gsWhereTermType {
        CreateBetween,
        CreateCompare,
        CreateExists,
        CreateExpression,
        CreateIn,
        CreateIsNotNull,
        CreateNotExists,
        CreateNotIn
    }

    internal enum gsCompareOperator {
        Equal = 0,
        NotEqual = 1,
        Greater = 2,
        Less = 3,
        LessOrEqual = 4,
        GreaterOrEqual = 5,
        BitwiseAnd = 6,
        Like = 7
    }
}
