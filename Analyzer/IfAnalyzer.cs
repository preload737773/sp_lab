using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Analyzer
{
    public class IfAnalyzer
    {
        /// <summary>
        /// Finds out whether the first statement was executed or not
        /// </summary>
        /// <param name="ifBodyText">If statement body</param>
        /// <returns>Whether the first statement was executed</returns>
        public string FindOutExecutedStatement(string ifBodyText)
        {
            StringBuilder result = new StringBuilder();
            var insertedText = ifBodyText.Contains("else")
                ? ifBodyText.Insert(ifBodyText.LastIndexOf("else {", StringComparison.Ordinal) + 6, "\n isFirst = false;")
                : ifBodyText;
            var code = "namespace IfCheck{" +
                       "public static class Analyzer{" +
                       "public static bool ifStatement(){" +
                       "bool isFirst = true;" +
                       insertedText +
                       "return isFirst;}}}";
            
            var tree = CSharpSyntaxTree.ParseText(code);
            string fileName = Guid.NewGuid().ToString();
            var systemRefLocation=typeof(object).GetTypeInfo().Assembly.Location;
            var systemReference = MetadataReference.CreateFromFile(systemRefLocation);
            var compilation = CSharpCompilation.Create(fileName)
                .WithOptions(
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(systemReference)
                .AddSyntaxTrees(tree);
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            EmitResult compilationResult = compilation.Emit(path);
            if(compilationResult.Success)
            {
                Assembly asm =
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
                bool isFirst = true;
                foreach (Type type in asm.GetExportedTypes())
                {
                    var checkIfStatementMethod = type.GetMethod("ifStatement");
                    isFirst = (bool) checkIfStatementMethod.Invoke(null, null);
                }
                result = isFirst
                    ? new StringBuilder("First statement has been executed.")
                    : new StringBuilder("Second statement has been executed.");
                return result.ToString();
            }
            foreach (Diagnostic codeIssue in compilationResult.Diagnostics)
            {
                string issue = $"Message: {codeIssue.GetMessage()}, At: {codeIssue.Location}";
                result.AppendLine(issue);
            }
            return result.ToString();
        }
    }
}