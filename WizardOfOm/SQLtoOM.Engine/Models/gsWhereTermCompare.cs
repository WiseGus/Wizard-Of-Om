using System;
using System.Text;

namespace SQLtoOM.Engine.Models {

    internal class gsWhereTermCompare : gsWhereTermBase {

        public gsSelectColumn FirstExpression { get; set; }
        public gsSelectColumn SecondExpression { get; set; }
        public gsCompareOperator Operator { get; set; }

        public override string ToString() {
            FirstExpression.ToStringUseExpression = true;
            SecondExpression.ToStringUseExpression = true;

            string firstExpressionStr = ParseExpression(FirstExpression);
            string secondExpressionStr = ParseExpression(SecondExpression);

            return $"WhereTerm.CreateCompare({firstExpressionStr}, {secondExpressionStr}, CompareOperator.{Operator.ToString()})";
        }

        private string ParseExpression(gsSelectColumn column) {
            string expressionStr;
            if (column is gsSubQueryColumn) {
                var subQryColumn = column as gsSubQueryColumn;
                subQryColumn.SubQuery.QryName = $"subQry{gsSelectQuery.GetNextID()}";

                InnerSqlOm += Environment.NewLine + subQryColumn.SubQuery.ToString();

                expressionStr = $"SqlExpression.SubQuery({subQryColumn.SubQuery.QryName})";
            }
            else {
                expressionStr = column.ToString();
            }

            return expressionStr;
        }
    }
}
