using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace IfAnalyzer
{
    public class Analyzer
    {
        /// <summary>
        /// Finds out whether the first statement was executed or not
        /// </summary>
        /// <param name="ifBodyText">If statement body</param>
        /// <returns>Whether the first statement was executed</returns>
        public string FindOutExecutedStatement(string ifBodyText)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            var insertedText = ifBodyText.Contains("else")
                ? ifBodyText.Insert(ifBodyText.LastIndexOf("else {", StringComparison.Ordinal) + 6, "\n isFirst = false;")
                : ifBodyText;
            var code = "namespace IfCheck{" +
                       "public static class Analyzer{" +
                       "public static bool ifStatement(){" +
                       "bool isFirst = true;" +
                       insertedText +
                       "return isFirst;}}}";
            var compilerResults = icc.CompileAssemblyFromSource(new CompilerParameters()
            {
                OutputAssembly = "checkIfStatement.dll",
                GenerateInMemory = true
            }, code);
            bool? isFirst = null;
            var dll = compilerResults.CompiledAssembly;
            foreach (var type in dll.GetExportedTypes())
            {
                var checkIfStatementMethod = type.GetMethod("ifStatement");
                isFirst = !(checkIfStatementMethod is null) && (bool) checkIfStatementMethod.Invoke(null, null);
            }
            if (isFirst == null)
            {
                return compilerResults.Output.ToString();
            }

            var result = isFirst.Value
                ? "First statement has been executed."
                : "Second statement has been executed.";
            return result;
        }
    }
}