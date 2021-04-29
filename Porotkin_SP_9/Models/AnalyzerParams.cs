namespace Porotkin_SP_9.Models
{
    public class AnalyzerParams
    {
        public string IfBodyText { get; set; }
    }

    public class AnalyzerResult
    {
        public string Result { get; }

        public AnalyzerResult(string result)
        {
            Result = result;
        }
    }
}