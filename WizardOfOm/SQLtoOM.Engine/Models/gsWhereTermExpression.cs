using System;

namespace SQLtoOM.Engine.Models {

    internal class gsWhereTermExpression : gsWhereTermBase {

        public string sql { get; set; }

        public override string ToString() {
            throw new NotImplementedException("WhereTermExpression not supported");
        }
    }
}
