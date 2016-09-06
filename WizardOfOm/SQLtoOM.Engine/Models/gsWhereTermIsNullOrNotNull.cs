namespace SQLtoOM.Engine.Models {

    internal class gsWhereTermIsNullOrNotNull : gsWhereTermBase {

        public gsNullOrNotNull NullType { get; set; }
        public gsSelectColumn Expression { get; set; }

        public override string ToString() {
            Expression.ToStringUseExpression = true;

            switch (NullType) {
                case gsNullOrNotNull.Null:
                    return $"WhereTerm.CreateIsNull({Expression.ToString()})";
                case gsNullOrNotNull.NotNull:
                    return $"WhereTerm.CreateIsNotNull({Expression.ToString()})";
            }

            return null;
        }
    }
}
