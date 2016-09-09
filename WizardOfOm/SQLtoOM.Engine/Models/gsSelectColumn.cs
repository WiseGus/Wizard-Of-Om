using System;

namespace SQLtoOM.Engine.Models {

    internal abstract class gsSelectColumn {

        private static int _ID = 0;

        public string ColumnName { get; set; }
        public string ColumnAlias { get; set; }
        public gsFromTerm Table { get; set; }        
        public object Value { get; set; }
        public bool ToStringUseExpression { get; set; }

        internal static int GetNextID() {
            return ++_ID;
        }

        internal static void ResetID() {
            _ID = 0;
        }

        public abstract override string ToString();
    }
}
