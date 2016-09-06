namespace SQLtoOM.Engine.Models {

    internal class gsWhereTermExistsOrNotExists : gsWhereTermBase {

        public string QueryName { get; set; }
        public string SqlName {
            get { return string.Concat(QueryName, "Sql"); }
        }
        public gsExistsOrNotExists ExistsType { get; set; }
        public string Sql { get; set; }

        public override string ToString() {
            return $"WhereTerm.{(ExistsType == gsExistsOrNotExists.Exists ? "CreateExists" : "CreateNotExists")}({SqlName})";
        }
    }
}
