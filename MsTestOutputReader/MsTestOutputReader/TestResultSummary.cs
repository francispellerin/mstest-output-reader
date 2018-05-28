namespace MsTestOutputReader
{
    public class TestResultSummary
    {
        public string Outcome { get; internal set; }
        public string Total { get; internal set; }
        public string Executed { get; internal set; }
        public string Passed { get; internal set; }
        public string Failed { get; internal set; }
    }
}