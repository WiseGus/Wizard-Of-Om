
namespace SQLtoOM.Engine.Models {
    internal class gsParameterColumn : gsSelectColumn {
        public override string ToString() {
            return $"SqlExpression.Parameter({ColumnName.Quoted()})";
        }
    }
}
