using ClientInformation.Core.Model;
using ClientInformation.Core.Services;
using GRYLibrary.Core.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientInformation.Tests.Testcases.Services
{
    [TestClass]
    public class ClientInformationServiceTests
    {
        [TestMethod]
        [TestProperty(nameof(TestKind), nameof(TestKind.UnitTest))]
        public void GetClientInformationTest()
        {
            // arrange
            string ip = "8.8.8.8";
            ClientInformationRecord expected = new ClientInformationRecord()
            {
                IPAddress = ip,
                Country = "US",
            };
            ClientInformationService clientInformationService = new ClientInformationService();

            // act
            ClientInformationRecord actual = clientInformationService.GetClientInformation(ip);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
