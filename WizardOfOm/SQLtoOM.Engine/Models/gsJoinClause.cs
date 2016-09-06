using System.Text;

namespace SQLtoOM.Engine.Models {

    internal class gsJoin {

        public string QryName { get; set; }
        public gsFromTerm TableA { get; set; }
        public gsFromTerm TableB { get; set; }
        public gsWhereClause JoinClause { get; set; }
        public gsJoinType JoinType { get; set; }

        public gsJoin() {
            JoinType = gsJoinType.Inner;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(JoinClause.ToString()).AppendLine();

            if (TableA is gsSubQueryFromTerm) {
                var subQry = TableA as gsSubQueryFromTerm;
                subQry.SubQuery.QryName = $"subQry{gsSelectQuery.GetNextID()}";

                sb.AppendLine();
                sb.AppendLine(subQry.SubQuery.ToString());
            }

            if (TableB is gsSubQueryFromTerm) {
                var subQry = TableB as gsSubQueryFromTerm;
                subQry.SubQuery.QryName = $"subQry{gsSelectQuery.GetNextID()}";

                sb.AppendLine();
                sb.AppendLine(subQry.SubQuery.ToString());
            }

            sb.AppendLine($"{QryName}.FromClause.Join(JoinType.{JoinType.ToString()}, {TableA.ToString()}, {TableB.ToString()}, {JoinClause.WhereClauseName});");
            return sb.ToString();
        }
    }
}
