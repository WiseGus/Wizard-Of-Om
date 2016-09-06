using System.Collections.Generic;
using System.Text;

namespace SQLtoOM.Engine.Models {

    internal class gsCaseColumn : gsSelectColumn {

        public string QueryName { get; set; }
        public string CaseOrigin { get; set; }
        public string CaseClauseName { get; set; }
        public string CaseAlias { get; set; }
        public IList<gsCaseTerm> CaseTerms { get; set; }
        public gsSelectColumn Else { get; set; }

        public gsCaseColumn() {
            CaseTerms = new List<gsCaseTerm>();
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            AddNestedCase(sb, this, CaseOrigin);

            sb.Append($"{QueryName}.Columns.Add(new SelectColumn(SqlExpression.Case({CaseClauseName}), {CaseAlias.Quoted()}));");

            return sb.ToString();
        }

        private void AddNestedCase(StringBuilder sb, gsCaseColumn caseColumn, string originCase) {
            caseColumn.CaseClauseName = "cc" + GetNextID();

            sb.AppendLine($"CaseClause {caseColumn.CaseClauseName} = new CaseClause();");

            foreach (gsCaseTerm caseTerm in caseColumn.CaseTerms) {
                sb.Append(caseTerm.When.ToString());

                string thenStr;
                if (caseTerm.Then is gsCaseColumn) {
                    var caseCol = caseTerm.Then as gsCaseColumn;
                    caseCol.CaseOrigin = originCase;

                    AddNestedCase(sb, caseCol, CaseClauseName);
                    thenStr = $"SqlExpression.Case({caseCol.CaseClauseName})";
                }
                else {
                    caseTerm.Then.ToStringUseExpression = true;
                    thenStr = caseTerm.Then.ToString();
                }
                sb.AppendLine($"{caseColumn.CaseClauseName}.Terms.Add(new CaseTerm({caseTerm.When.WhereClauseName}, {thenStr}));");
            }

            caseColumn.CaseAlias = ColumnAlias;
            if (caseColumn.Else.ColumnIsValid()) {
                caseColumn.CaseAlias = caseColumn.Else.ColumnAlias;
                caseColumn.Else.ColumnAlias = null; // Messes up the case ElseValue
                sb.AppendLine($"{caseColumn.CaseClauseName}.ElseValue = {caseColumn.Else.ToString()};");
            }
        }
    }
}