using System;
using System.Text;

namespace SQLtoOM.Engine.Models {

    internal class gsSubQueryColumn : gsSelectColumn {

        public gsSelectQuery SubQuery { get; set; }

        public override string ToString() {
            return $"SqlExpression.SubQuery({SubQuery.QryName})";
        }
    }
}
