using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MsTestOutputReader.Entities;

namespace MsTestOutputReader
{
    public class MsTestResultReader : ITestResultReader
    {
        public TestResult ReadTestResultFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return new TestResult();

            var directory = new DirectoryInfo(path);

            var trxFiles = directory.GetFiles("*.trx", SearchOption.AllDirectories);

            var result = new TestResult { Outcome = "Passed", Tests = new List<UnitTestResult>() };

            foreach (var file in trxFiles)
            {
                var testResult = ReadTestResultFromFile(file.FullName);

                if (testResult.Outcome == "Failed")
                    result.Outcome = "Failed";

                result.Total += testResult.Total;
                result.Executed += testResult.Executed;
                result.Passed += testResult.Passed;
                result.Failed += testResult.Failed;
                result.Tests.AddRange(testResult.Tests);
            }

            return result;
        }

        public TestResult ReadTestResultFromFile(string fileFullName)
        {
            if (string.IsNullOrWhiteSpace(fileFullName))
                return new TestResult();

            var xml = GetXml(fileFullName);

            return ReadTestResult(xml);
        }

        public TestResult ReadTestResult(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return null;

            var summary = ParseTestResultSummary(xml);


            return new TestResult
            {
                Outcome = summary.Outcome,
                Total = Convert.ToInt32(summary.Total),
                Executed = Convert.ToInt32(summary.Executed),
                Passed = Convert.ToInt32(summary.Passed),
                Failed = Convert.ToInt32(summary.Failed),
                Tests = ParseUnitTestResult(xml)
            };
        }

        private string GetXml(string fileFullname)
        {
            var xml = File.ReadAllText(fileFullname);

            return xml;
        }

        private static TestResultSummary ParseTestResultSummary(string xml)
        {
            var doc = XDocument.Parse(xml);

            var resultSummaryNode = doc.Descendants().First(d => d.Name.LocalName == "ResultSummary");

            var counters = resultSummaryNode.Descendants().First(d => d.Name.LocalName == "Counters");
            
            return new TestResultSummary
            {
                Outcome = resultSummaryNode.Attributes().First(a => a.Name.LocalName == "outcome").Value,
                Total = counters.Attributes().First(a => a.Name.LocalName == "total").Value,
                Executed = counters.Attributes().First(a => a.Name.LocalName == "executed").Value,
                Passed = counters.Attributes().First(a => a.Name.LocalName == "passed").Value,
                Failed = counters.Attributes().First(a => a.Name.LocalName == "failed").Value
            };
        }

        private static List<UnitTestResult> ParseUnitTestResult(string xml)
        {
            var doc = XDocument.Parse(xml);

            var testResultsNode = doc.Descendants().First(d => d.Name.LocalName == "Results");

            List<UnitTestResult> unitTestsResults = new List<UnitTestResult>();
            foreach (var result in testResultsNode.Descendants().Where(d => d.Name.LocalName == "UnitTestResult"))
            {
                UnitTestResult unitTestResult = new UnitTestResult();

                unitTestResult.TestName = result.Attributes().First(a => a.Name.LocalName == "testName").Value;
                unitTestResult.StartTime = result.Attributes().First(a => a.Name.LocalName == "startTime").Value;
                unitTestResult.EndTime = result.Attributes().First(a => a.Name.LocalName == "endTime").Value;
                unitTestResult.Outcome = result.Attributes().First(a => a.Name.LocalName == "outcome").Value;

                unitTestsResults.Add(unitTestResult);
            }

            return unitTestsResults;
        }
    }
}
