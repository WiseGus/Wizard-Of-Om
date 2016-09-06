
namespace SQLtoOM.Engine.Models {
    internal class gsNumberColumn : gsSelectColumn {
        public override string ToString() {
            if (ColumnAlias.HasValue()) {
                return $"SqlExpression.Number({Value}), {ColumnAlias.Quoted()}";
            }
            else {
                return $"SqlExpression.Number({Value})";
            }
        }
    }
}
