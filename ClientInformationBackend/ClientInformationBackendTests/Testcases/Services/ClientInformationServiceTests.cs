using ClientInformationBackend.Core.Configuration;
using ClientInformationBackend.Core.Model;
using ClientInformationBackend.Core.Services;
using GRYLibrary.Core.APIServer.Settings.Configuration;
using GRYLibrary.Core.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            Mock<IPersistedAPIServerConfiguration<CodeUnitSpecificConfiguration>> persistedAPIServerConfigurationMock = new Mock<IPersistedAPIServerConfiguration<CodeUnitSpecificConfiguration>>(MockBehavior.Strict);
            persistedAPIServerConfigurationMock.SetupGet(mock => mock.ApplicationSpecificConfiguration).Returns(codeUnitSpecificConfiguration);
            ClientInformationBackendRecord expected = new ClientInformationBackendRecord(ip)
            {
                Country = "US",
                Contact = contactInformation,
                LicenseInformation = licenseInformation,
            };
            ClientInformationBackendService ClientInformationBackendService = new ClientInformationBackendService(persistedAPIServerConfigurationMock.Object);

            // act
            ClientInformationBackendRecord actual = ClientInformationBackendService.GetClientInformationBackend(ip);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
