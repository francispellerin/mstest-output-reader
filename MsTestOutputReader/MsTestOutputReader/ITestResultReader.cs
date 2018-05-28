using MsTestOutputReader.Entities;

namespace MsTestOutputReader
{
    interface ITestResultReader
    {
        TestResult ReadTestResult(string xml);
        TestResult ReadTestResultFromFile(string fileFullName);
        TestResult ReadTestResultFromPath(string path);
    }
}
