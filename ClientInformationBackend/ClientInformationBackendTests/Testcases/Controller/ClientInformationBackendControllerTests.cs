using ClientInformation.Backend.Core.Controller;
using ClientInformation.Library.Core.Model;
using ClientInformation.Library.Core.Services;
using GRYLibrary.Core.Exceptions;
using GRYLibrary.Core.Misc;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;
using System.Text.Json;

namespace ClientInformation.Backend.Tests.Testcases.Controller
{
    [TestClass]
    public class ClientInformationBackendControllerTests
    {
        [TestMethod]
        [TestProperty(nameof(TestKind), nameof(TestKind.UnitTest))]
        public void TestGetClientIPAddressReturnsIPFromHttpContext()
        {
            // arrange
            string expected = "8.8.8.8";
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.Items["ClientIPAddress"] = IPAddress.Parse(expected);

            // act
            string actual = ClientInformationBackendController.GetClientIPAddress(httpContext);

            // assert
            Assert.AreEqual(expected, actual);
        }

     

        [TestMethod]
        [TestProperty(nameof(TestKind), nameof(TestKind.UnitTest))]
        public void TestCalculateResponseForClientInformationRequestSerializesRecord()
        {
            // arrange
            string ip = "8.8.8.8";
            ClientInformationBackendRecord record = new ClientInformationBackendRecord()
            {
                IPAddress = ip,
                Country = "US",
            };
            Mock<IClientInformationBackendService> serviceMock = new Mock<IClientInformationBackendService>();
            serviceMock.Setup(service => service.GetClientInformationRecord(ip)).Returns(record);
            string expected = JsonSerializer.Serialize(record, new JsonSerializerOptions { WriteIndented = true });

            // act
            string actual = ClientInformationBackendController.CalculateResponseForClientInformationRequest(ip, serviceMock.Object);

            // assert
            Assert.AreEqual(expected, actual);
            serviceMock.Verify(service => service.GetClientInformationRecord(ip), Times.Once);
        }
    }
}
