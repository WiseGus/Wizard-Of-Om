using Microsoft.CSharp;
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

namespace SQLtoOM.Engine.Compilers {
    internal class ExecutronCompiler : ICompiler {

        public bool HasError { get; set; }

        public string Result { get; set; }

        public void Compile(string sourceCode) {
            Assembly compiledAssembly = CompileSourceCodeDom(sourceCode);
            if (compiledAssembly == null) return;

            try {
                Type type = compiledAssembly.GetType("Executron");
                var instance = compiledAssembly.CreateInstance("Executron");
                MethodInfo methodInfo = type.GetMethod("Execute");
                var res = methodInfo.Invoke(instance, null);
                Result = Convert.ToString(res);
            }
            catch (Exception ex) {
                Result = ex.ToString();
                HasError = true;
            }
        }

        private Assembly CompileSourceCodeDom(string sourceCode) {
            CodeDomProvider csharpCodeProvider = new CSharpCodeProvider();
            var cp = new CompilerParameters();

            // Add system assemblies
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            cp.ReferencedAssemblies.Add(executingAssembly.Location);

            foreach (AssemblyName assemblyName in executingAssembly.GetReferencedAssemblies()) {
                cp.ReferencedAssemblies.Add(Assembly.Load(assemblyName).Location);
            }

            // Add external assemblies
            //IEnumerable<string> references = GetReferences();
            //foreach (string reference in references) {
            //    cp.ReferencedAssemblies.Add(reference);
            //}
            cp.ReferencedAssemblies.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlOM.dll"));

            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;

            CompilerResults cr = csharpCodeProvider.CompileAssemblyFromSource(cp, sourceCode);

            if (cr.Errors.HasErrors) {
                Result = string.Join(Environment.NewLine, cr.Errors.Cast<CompilerError>().Select(p => p.ToString()));
                HasError = true;
                return null;
            }

            return cr.CompiledAssembly;
        }

        //private IEnumerable<string> GetReferences() {
        //    List<string> assemblies = new List<string>();

        //    string assembliesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assemblies");
        //    Directory.CreateDirectory(assembliesPath);
        //    var files = Directory.GetFiles(assembliesPath, "*.dll");
        //    assemblies.AddRange(files);

        //    return assemblies;
        //}
    }
}