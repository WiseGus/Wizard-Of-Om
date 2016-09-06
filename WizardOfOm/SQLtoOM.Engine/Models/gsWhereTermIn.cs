using System.Collections.Generic;

namespace SQLtoOM.Engine.Models {

    internal class gsWhereTermIn : gsWhereTermBase {

        public string QueryName { get; set; }
        public string SqlName
        {
            get { return string.Concat(QueryName, "Sql"); }
        }
        public gsInOrNotIn InType { get; set; }
        public gsSelectColumn Expression { get; set; }
        public string Sql { get; set; }
        public List<string> values { get; set; }

        public override string ToString() {
            Expression.ToStringUseExpression = true;

            if (!string.IsNullOrEmpty(Sql)) {
                return $"WhereTerm.{(InType == gsInOrNotIn.In ? "CreateIn" : "CreateNotIn")}({Expression.ToString()}, {SqlName})";
            }
            else {
                string valuesListAsText;
                for (int i = 0; i < values.Count; i++) {
                    values[i] = values[i].Quoted();
                }
                valuesListAsText = "new[] { " + string.Join(", ", values) + " }";
                return $"WhereTerm.{(InType == gsInOrNotIn.In ? "CreateIn" : "CreateNotIn")}({Expression.ToString()}, SqlConstantCollection.FromList({valuesListAsText}))";
            }
        }
    }
}
