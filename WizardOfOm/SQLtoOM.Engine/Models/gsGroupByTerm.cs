namespace SQLtoOM.Engine.Models {

    internal class gsGroupByTerm {

        public string Field { get; set; }
        public gsFromTerm Table { get; set; }
        
        public override string ToString() {
            if (Table != null && Table.TableName.HasValue()) {
                return $"new GroupByTerm({Field.Quoted()}, {Table.ToString()})";
            }
            else {
                return $"new GroupByTerm({Field.Quoted()})";
            }
        }
    }
}
