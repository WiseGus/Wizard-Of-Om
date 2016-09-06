namespace SQLtoOM.Engine.Compilers {

    public interface ICompiler {

        bool HasError { get; set; }
        string Result { get; set; }

        void Compile(string sourceCode);
    }
}