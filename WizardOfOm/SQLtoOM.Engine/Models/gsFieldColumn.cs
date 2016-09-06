
namespace SQLtoOM.Engine.Models {
    internal class gsFieldColumn : gsSelectColumn {

        public override string ToString() {
            if (ToStringUseExpression) {
                return $"SqlExpression.Field({ColumnName.Quoted()}{(Table != null ? ", " + Table.ToString() : string.Empty)})";
            }
            else {
                if (Table == null && ColumnAlias.HasValue()) {
                    return $"SqlExpression.Field({ColumnName.Quoted()}){(ColumnAlias.HasValue() ? ", " + ColumnAlias.Quoted() : string.Empty)}";
                }
                else {
                    return $"{ColumnName.Quoted()}{(Table != null ? ", " + Table.ToString() : string.Empty)}{(ColumnAlias.HasValue() ? ", " + ColumnAlias.Quoted() : string.Empty)}";
                }
            }
        }
    }
}