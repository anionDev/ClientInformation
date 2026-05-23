using ClientInformationBackend.Core.Configuration;
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
            string contactInformation = "some contact-information";
            string licenseInformation = "some license-information";
            CodeUnitSpecificConfiguration codeUnitSpecificConfiguration = new CodeUnitSpecificConfiguration()
            {
                ContactInformation = contactInformation,
                LicenseInformation = licenseInformation,
            };
            ClientInformation.Library.Core.Model.ClientInformationBackendRecord expected = new ClientInformation.Library.Core.Model.ClientInformationBackendRecord()
            {
                IPAddress = ip,
                Country = "US",
            };
            ClientInformation.Library.Core.Services.IClientInformationBackendService ClientInformationBackendService = new ClientInformation.Library.Core.Services.ClientInformationBackendService();

            // act
            ClientInformation.Library.Core.Model.ClientInformationBackendRecord actual = ClientInformationBackendService.GetClientInformationRecord(ip);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
