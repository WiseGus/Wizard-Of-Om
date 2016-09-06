using Reeb.SqlOM;

namespace SQLtoOM.Engine.Models {

    internal class gsFromTerm {
        public string TableName { get; set; }
        public string TableAlias { get; set; }

        public override string ToString() {
            if (TableAlias.HasValue()) {
                return $"FromTerm.Table({TableName.Quoted()}, {TableAlias.Quoted()})";
            }
            else {
                return $"FromTerm.Table({TableName.Quoted()})";
            }
        }
    }
}
