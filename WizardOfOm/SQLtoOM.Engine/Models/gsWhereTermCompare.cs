namespace SQLtoOM.Engine.Models {

    internal class gsWhereTermCompare : gsWhereTermBase {

        public gsSelectColumn FirstExpression { get; set; }
        public gsSelectColumn SecondExpression { get; set; }
        public gsCompareOperator Operator { get; set; }

        public override string ToString() {
            FirstExpression.ToStringUseExpression = true;
            SecondExpression.ToStringUseExpression = true;

            string firstExpressionStr = FirstExpression.ToString();
            string secondExpressionStr = SecondExpression.ToString();

            return $"WhereTerm.CreateCompare({firstExpressionStr}, {secondExpressionStr}, CompareOperator.{Operator.ToString()})";
        }
    }
}
