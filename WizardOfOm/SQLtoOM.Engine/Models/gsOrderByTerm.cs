namespace SQLtoOM.Engine.Models {

    internal class gsOrderByTerm {

        public gsOrderByDirection Direction { get; set; }
        public string Field { get; set; }
        public gsFromTerm Table { get; set; }

        public override string ToString() {
            if (Table != null && Table.TableName.HasValue()) {
                return $"new OrderByTerm({Field.Quoted()}, {Table.ToString()}, OrderByDirection.{Direction.ToString()})";
            }
            else {
                return $"new OrderByTerm({Field.Quoted()}, OrderByDirection.{Direction.ToString()})";
            }
        }
    }
}
