namespace SQLtoOM.Engine.SQLParsers {

    public interface ISQLParser {
        
        string GeneratedSqlOm { get; }

        void ParseSQL(string sql);
    }
}