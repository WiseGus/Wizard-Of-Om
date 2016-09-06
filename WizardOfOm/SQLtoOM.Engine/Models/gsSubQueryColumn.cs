namespace SQLtoOM.Engine.Models {

    internal class gsSubQueryColumn : gsSelectColumn {

        public gsSelectQuery SubQuery { get; set; }
    }
}
