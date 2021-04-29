namespace Porotkin_SP_9.Models
{
    public class LowLevelParams
    {
        public int First { get; set; }
        public int Second { get; set; }
    }

    public class LowLevelResult
    {
        public string Result { get; }

        public LowLevelResult(string result)
        {
            Result = result;
        }
    }
}