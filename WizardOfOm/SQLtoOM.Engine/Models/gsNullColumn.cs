
namespace SQLtoOM.Engine.Models {
    internal class gsNullColumn : gsSelectColumn {

        public override string ToString() {
            if (ToStringUseExpression || ColumnAlias.HasValue()) {
                return $"SqlExpression.Null(), {ColumnAlias.Quoted()}";
            }
            else {
                return "SqlExpression.Null()";
            }
        }
    }
}
