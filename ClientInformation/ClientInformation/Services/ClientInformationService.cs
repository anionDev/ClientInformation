
using ClientInformation.Core.Model;
using GRYLibrary.Core.Exceptions;
using System.Net;

namespace ClientInformation.Core.Services
{
    public class ClientInformationService : IClientInformationService
    {
        public ClientInformationRecord GetClientInformation(string ipAddress)
        {
            if (!this.IsValidIPAddress(ipAddress))
            {
                throw new BadRequestException($"Invalid IP-Address: \"{ipAddress}\"");
            }
            return new ClientInformationRecord()
            {
                IPAddress = ipAddress,
                Country = null,//TODO
                IsPingable = false,//TODO
                Contact = null,//TODO
                LicenseInformation = null,//TODO
            };
        }

        private bool IsValidIPAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }
            return IPAddress.TryParse(ipAddress, out _);
        }
    }
}
