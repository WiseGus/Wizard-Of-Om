using Reeb.SqlOM;
using System.Collections.Generic;
using System.Text;

namespace SQLtoOM.Engine.Models {

    internal class gsSelectQuery {

        private static int _ID = 0;

        public string QryName { get; set; }
        public int Top { get; set; }
        public bool Distinct { get; set; }
        public gsFromClause FromClause { get; set; }
        public IList<gsSelectColumn> Columns { get; set; }
        public IList<gsJoin> Joins { get; set; }
        public IList<gsGroupByTerm> GroupByTerms { get; set; }
        public bool GroupByWithCube { get; set; }
        public bool GroupByWithRollup { get; set; }
        public gsWhereClause WhereClause { get; set; }
        public IList<gsOrderByTerm> OrderByTerms { get; set; }
        public gsWhereClause HavingPhrase { get; set; }

        public gsSelectQuery() {
            QryName = "qry";
            Columns = new List<gsSelectColumn>();
            FromClause = new gsFromClause();
            Joins = new List<gsJoin>();
            GroupByTerms = new List<gsGroupByTerm>();
            HavingPhrase = new gsWhereClause();
            OrderByTerms = new List<gsOrderByTerm>();
            WhereClause = new gsWhereClause();
        }

        internal static int GetNextID() {
            return ++_ID;
        }

        internal static void ResetID() {
            _ID = 0;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            AddSelectQuery(sb);
            AddTop(sb);
            AddDistinct(sb);
            AddBaseTable(sb);
            AddColumns(sb);
            AddJoins(sb);
            AddWhere(sb);
            AddOrderBy(sb);
            AddHaving(sb);
            AddGroupBy(sb);

            string finalSqlOmText = sb.ToString();

            return finalSqlOmText;
        }

        private void AddSelectQuery(StringBuilder sb) {
            sb.AppendLine($"SelectQuery {QryName} = new SelectQuery();");
        }

        private void AddTop(StringBuilder sb) {
            if (Top == 0) return;

            sb.AppendLine()
              .AppendLine($"{QryName}.Top = {Top};");
        }

        private void AddDistinct(StringBuilder sb) {
            if (!Distinct) return;

            sb.AppendLine()
              .AppendLine($"{QryName}.Distinct = true;");
        }

        private void AddBaseTable(StringBuilder sb) {
            sb.AppendLine();

            if (FromClause.BaseTable is gsSubQueryFromTerm) {
                gsSelectQuery subQry = (FromClause.BaseTable as gsSubQueryFromTerm).SubQuery;
                subQry.QryName = $"subQry{GetNextID()}";

                sb.AppendLine();
                sb.AppendLine(subQry.ToString());
                string subQueryStr = $"FromTerm.SubQuery({subQry.QryName}, {FromClause.BaseTable.TableAlias.Quoted()})";
                sb.Append($"{QryName}.FromClause.BaseTable = {subQueryStr};");
            }
            else {
                sb.Append($"{QryName}.FromClause.BaseTable = {FromClause.BaseTable.ToString()};");
            }
            sb.AppendLine();
        }

        private void AddColumns(StringBuilder sb) {
            if (Columns.Count == 0) return;

            sb.AppendLine();
            foreach (gsSelectColumn selCol in Columns) {
                if (selCol is gsCaseColumn) {
                    gsCaseColumn caseColumn = selCol as gsCaseColumn;
                    caseColumn.QueryName = QryName;

                    sb.Append(caseColumn.ToString());
                }
                else if (selCol is gsSubQueryColumn) {
                    gsSelectQuery subQry = (selCol as gsSubQueryColumn).SubQuery;
                    subQry.QryName = $"subQry{GetNextID()}";

                    sb.AppendLine();
                    sb.AppendLine(subQry.ToString());
                    sb.Append($"{QryName}.Columns.Add(new SelectColumn(SqlExpression.SubQuery({subQry.QryName}), {selCol.ColumnAlias.Quoted()}));");
                }
                else {
                    sb.Append($"{QryName}.Columns.Add(new SelectColumn({selCol.ToString()}));");
                }
                sb.AppendLine();
            }
        }

        private void AddJoins(StringBuilder sb) {
            if (Joins.Count == 0) return;

            foreach (gsJoin joinClause in Joins) {
                joinClause.QryName = QryName;
                sb.Append($"{joinClause.ToString()}");
            }
        }

        private void AddWhere(StringBuilder sb) {
            if (WhereClause.Terms.Count == 0 && WhereClause.SubClauses.Count == 0) return;

            WhereClause.WhereClauseOrigin = $"{QryName}.WherePhrase";
            sb.AppendLine(WhereClause.ToString());
        }

        private void AddOrderBy(StringBuilder sb) {
            if (OrderByTerms.Count == 0) return;

            sb.AppendLine();
            foreach (gsOrderByTerm orderByTerm in OrderByTerms) {
                sb.AppendLine($"{QryName}.OrderByTerms.Add({orderByTerm.ToString()});");
            }
        }

        private void AddGroupBy(StringBuilder sb) {
            if (GroupByTerms.Count == 0) return;

            sb.AppendLine();
            foreach (gsGroupByTerm groupByTerm in GroupByTerms) {
                sb.AppendLine($"{QryName}.GroupByTerms.Add({groupByTerm.ToString()});");
            }
        }

        private void AddHaving(StringBuilder sb) {
            if (HavingPhrase.Terms.Count == 0 && HavingPhrase.SubClauses.Count == 0) return;

            HavingPhrase.WhereClauseOrigin = $"{QryName}.HavingPhrase";
            sb.AppendLine(HavingPhrase.ToString());
        }
    }
}
