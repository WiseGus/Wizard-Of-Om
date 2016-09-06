using System;

namespace SQLtoOM.Engine.Models {

    internal class gsWhereTermBetween : gsWhereTermBase {

        public gsSelectColumn Expression { get; set; }
        public gsSelectColumn LowBoundExpression { get; set; }
        public gsSelectColumn HighBoundExpression { get; set; }

        public override string ToString() {
            throw new NotImplementedException("WhereTermBetween not supported");
        }
    }
}
