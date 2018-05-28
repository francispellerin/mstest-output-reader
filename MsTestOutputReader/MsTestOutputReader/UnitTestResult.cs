namespace MsTestOutputReader
{
    public class UnitTestResult
    {
        public string TestName { get; internal set; }
        public string StartTime { get; internal set; }
        public string EndTime { get; internal set; }
        public string Outcome { get; internal set; }
    }
}