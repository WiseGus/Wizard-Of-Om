using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLtoOM.Engine.Models {

    internal class gsUnaryColumn : gsSelectColumn {
        public gsUnaryType UnaryType { get; set; }


        public override string ToString() {
            throw new NotImplementedException();
        }
    }
}
