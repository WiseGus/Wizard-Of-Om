using Microsoft.SqlServer.TransactSql.ScriptDom;
using SQLtoOM.Engine.Models;

namespace SQLtoOM.Engine.gsQueryParsers {
    internal abstract class gsColumnParserBase {

        public gsColumnParserBase(SelectElement element) {
        }

        public abstract gsSelectColumn Parse();
    }
}
