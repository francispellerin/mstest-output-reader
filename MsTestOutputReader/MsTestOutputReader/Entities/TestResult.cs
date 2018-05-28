using System.Collections.Generic;

namespace MsTestOutputReader.Entities
{
    public class TestResult
    {
        public string Outcome { get; set; }
        public int Total { get; set; }
        public int Executed { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public List<UnitTestResult> Tests { get; internal set; }
    }
}