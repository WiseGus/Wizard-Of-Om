
namespace SQLtoOM.Engine.Models {
    internal class gsRawColumn : gsSelectColumn {
        public override string ToString() {
            return $"SqlExpression.Parameter({ColumnName.Quoted()})";
        }
    }
}
