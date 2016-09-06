using SQLtoOM.Engine.Compilers;
using SQLtoOM.Engine.SQLParsers;
using System;
using System.Reflection;

namespace SQLtoOM.Engine {

    public class SqlOMEngine {

        private ISQLParser _Parser;
        private ICompiler _Compiler;

        public SqlOMEngine() {
        }

        public SqlOMEngine(ISQLParser parser, ICompiler compiler) {
            _Parser = parser;
            _Compiler = compiler;
        }

        public void ParseSQL(RendererType rendererType, string sql) {
            if (_Parser == null) {
                _Parser = new TSqlParser();
            }
            if (_Compiler == null) {
                _Compiler = new ExecutronCompiler();
            }

            _Parser.ParseSQL(sql);

            CompileGeneratedSqlOm(rendererType);
        }

        private void CompileGeneratedSqlOm(RendererType rendererType) {
            string textToCompile = @"using System;
                                     using Reeb.SqlOM;

                                     public class Executron {
                                         public string Execute() {
#QUERY#
#RENDERER#                                             
                                             return renderer.RenderSelect(qry);
                                         }
                                     }";

            textToCompile = textToCompile.Replace("#QUERY#", _Parser.GeneratedSqlOm);
            textToCompile = textToCompile.Replace("#RENDERER#", GetRendererTypeStr(rendererType));

            _Compiler.Compile(textToCompile);

            if (_Compiler.HasError) {
                throw new Exception(string.Concat(_Compiler.Result, Environment.NewLine, Environment.NewLine, "SOURCE:", Environment.NewLine, _Parser.GeneratedSqlOm));
            }
        }

        private string GetRendererTypeStr(RendererType rendererType) {
            switch (rendererType) {
                case RendererType.MsSQL:
                    return "var renderer = new Reeb.SqlOM.Render.SqlServerRenderer();";
                case RendererType.Oracle:
                    return "var renderer = new Reeb.SqlOM.Render.OracleRenderer();";
                default: throw new NotImplementedException("Invalid renderer");
            }
        }

        public string GetGeneratedSqlOm() {
            return _Parser.GeneratedSqlOm;
        }

        public string GetGeneratedSql() {
            return _Compiler.Result;
        }

        public static string GetVersion() {
            Assembly assem = Assembly.GetExecutingAssembly();
            AssemblyName assemName = assem.GetName();
            return string.Concat(assemName.Version.Major, ".", assemName.Version.Minor);
        }
    }
}
