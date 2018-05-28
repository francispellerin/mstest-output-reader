using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsTestOutputReader;
using System.IO;

namespace MsTestOutputReaderTests
{
    [TestClass]
    public class XmlParserTests
    {
        [TestMethod]
        public void Read_Test_Results_From_Empty_Xml_Returns_Null()
        {
            var reader = new MsTestResultReader();

            var result = reader.ReadTestResult(string.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Read_Test_Results_From_Xml_Returns_Test_Run_Summary()
        {
            var reader = new MsTestResultReader();

            var result = reader.ReadTestResult(Xml());

            Assert.IsNotNull(result);
            Assert.AreEqual("Failed", result.Outcome);
            Assert.AreEqual(2, result.Total);
            Assert.AreEqual(2, result.Executed);
            Assert.AreEqual(1, result.Passed);
            Assert.AreEqual(1, result.Failed);
        }

        [TestMethod]
        public void Read_Test_Results_From_Xml_Returns_Tests_List()
        {
            var reader = new MsTestResultReader();

            var result = reader.ReadTestResult(Xml());

            Assert.AreEqual(2, result.Tests.Count);
        }

        [TestMethod]
        public void Read_Test_Results_From_File()
        {
            var reader = new MsTestResultReader();

            var result = reader.ReadTestResultFromFile("samples\\testresult.trx");

            Assert.AreEqual(2, result.Tests.Count);
        }

        [TestMethod]
        public void Read_Test_Results_From_Path()
        {
            var reader = new MsTestResultReader();

            var result = reader.ReadTestResultFromPath("samples");

            Assert.AreEqual("Failed", result.Outcome);
            Assert.AreEqual(4, result.Total);
            Assert.AreEqual(4, result.Executed);
            Assert.AreEqual(2, result.Passed);
            Assert.AreEqual(2, result.Failed);
            Assert.AreEqual(4, result.Tests.Count);
        }

        private string Xml()
        {
            var xml = File.ReadAllText("samples\\testresult.trx");

            return xml;
        }
    }
}
