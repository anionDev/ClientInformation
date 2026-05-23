using ClientInformation.Library.Core.Model;
using ClientInformation.Library.Core.Services;
using GRYLibrary.Core.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientInformationLibrary.Tests.Testcases.Services
{
    [TestClass]
    public class ClientInformationBackendServiceTests
    {
        [TestMethod]
        [TestProperty(nameof(TestKind), nameof(TestKind.UnitTest))]
        public void TestGetClientInformationRecordForKnownIP()
        {
            // arrange
            string ip = "8.8.8.8";
            ClientInformationBackendRecord expected = new ClientInformationBackendRecord()
            {
                IPAddress = ip,
                Country = "US",
            };
            IClientInformationBackendService service = new ClientInformationBackendService();

            // act
            ClientInformationBackendRecord actual = service.GetClientInformationRecord(ip);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
