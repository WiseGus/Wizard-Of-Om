using System.Collections.Generic;
using System.Text;

namespace SQLtoOM.Engine.Models {
    internal class gsWhereClause {

        private static int _ID = 0;

        public string WhereClauseOrigin { get; set; }
        public string WhereClauseName { get; private set; }
        public gsWhereClauseRelationship Relationship { get; set; }
        public IList<gsWhereClause> SubClauses { get; set; }
        public IList<gsWhereTermBase> Terms { get; set; }

        public gsWhereClause() {
            Relationship = gsWhereClauseRelationship.And;
            SubClauses = new List<gsWhereClause>();
            Terms = new List<gsWhereTermBase>();
        }

        internal static int GetNextID() {
            return ++_ID;
        }

        internal static void ResetID() {
            _ID = 0;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            AddNestedWhere(sb, this, WhereClauseOrigin);

            return sb.ToString();
        }

        private void AddNestedWhere(StringBuilder sb, gsWhereClause whereClause, string originWhereClauseName) {
            if (whereClause.Terms.Count == 0 && whereClause.SubClauses.Count == 0) return;

            WhereClauseName = "wc" + GetNextID();

            string tmpWhereClauseName = WhereClauseName;

            sb.AppendLine()
              .AppendLine($"WhereClause {tmpWhereClauseName} = new WhereClause(WhereClauseRelationship.{whereClause.Relationship.ToString()});");

            foreach (gsWhereTermBase whereTerm in whereClause.Terms) {
                if (whereTerm is gsWhereTermExistsOrNotExists) {
                    var whereExists = whereTerm as gsWhereTermExistsOrNotExists;

                    sb.AppendLine(whereExists.Sql)
                      .AppendLine($"string {whereExists.SqlName} = new Reeb.SqlOM.Render.SqlServerRenderer().RenderSelect({whereExists.QueryName});");
                }
                else if (whereTerm is gsWhereTermIn) {
                    var whereIn = whereTerm as gsWhereTermIn;
                    if (!string.IsNullOrEmpty(whereIn.Sql)) {
                        sb.AppendLine((whereTerm as gsWhereTermIn).Sql)
                          .AppendLine($"string {whereIn.SqlName} = new Reeb.SqlOM.Render.SqlServerRenderer().RenderSelect({whereIn.QueryName});");
                    }
                }

                sb.AppendLine($"{tmpWhereClauseName}.Terms.Add({whereTerm.ToString()});");
            }

            foreach (gsWhereClause subClause in whereClause.SubClauses) {
                subClause.WhereClauseOrigin = tmpWhereClauseName;
                AddNestedWhere(sb, subClause, subClause.WhereClauseOrigin);
            }

            if (originWhereClauseName.HasValue()) {
                sb.AppendLine()
                  .Append($"{originWhereClauseName}.SubClauses.Add({tmpWhereClauseName});");
            }
        }
    }
}
