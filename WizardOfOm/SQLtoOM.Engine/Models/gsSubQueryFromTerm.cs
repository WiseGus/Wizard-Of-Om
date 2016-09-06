
using System.Text;

namespace SQLtoOM.Engine.Models {
    internal class gsSubQueryFromTerm : gsFromTerm {

        public gsSelectQuery SubQuery { get; set; }

        public override string ToString() {
            return $"FromTerm.SubQuery({SubQuery.QryName}, {TableAlias.Quoted()})";
        }
    }
}
