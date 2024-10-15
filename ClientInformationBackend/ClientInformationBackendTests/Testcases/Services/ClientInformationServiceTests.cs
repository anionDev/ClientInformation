using ClientInformationBackend.Core.Model;
using ClientInformationBackend.Core.Services;
using GRYLibrary.Core.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientInformationBackend.Tests.Testcases.Services
{
    [TestClass]
    public class ClientInformationBackendServiceTests
    {
        [TestMethod]
        [TestProperty(nameof(TestKind), nameof(TestKind.UnitTest))]
        public void GetClientInformationBackendTest()
        {
            // arrange
            string ip = "8.8.8.8";
            ClientInformationBackendRecord expected = new ClientInformationBackendRecord()
            {
                IPAddress = ip,
                Country = "US",
                Contact = "See /API/Other/Resources/Information/Contact for contact-information",
                LicenseInformation = "See /API/Other/Resources/Information/License for license-information",
            };
            ClientInformationBackendService ClientInformationBackendService = new ClientInformationBackendService();

            // act
            ClientInformationBackendRecord actual = ClientInformationBackendService.GetClientInformationBackend(ip);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
