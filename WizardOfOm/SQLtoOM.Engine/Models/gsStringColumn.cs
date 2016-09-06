
namespace SQLtoOM.Engine.Models {
    internal class gsStringColumn : gsSelectColumn {
        public override string ToString() {
            if (ColumnAlias.HasValue()) {
                return $"SqlExpression.String({Value.ToString().Quoted()}), {ColumnAlias.Quoted()}";
            }
            else {
                return $"SqlExpression.String({Value.ToString().Quoted()})";
            }
        }
    }
}
