
namespace SQLtoOM.Engine.Models {
    internal class gsDateColumn : gsSelectColumn {

        public override string ToString() {
            return $"SqlExpression.Parameter({ColumnName.Quoted()})";
        }
    }
}
