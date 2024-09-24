
namespace ClientInformation.Core.Services
{
    public class ClientInformationService : IClientInformationService
    {
        public Model.ClientInformationRecord GetClientInformation(string ipAddress)
        {
            return new Model.ClientInformationRecord()
            {
                IPAddress = ipAddress,
                Country = null,//TODO
                IsPingable = false,//TODO
                Contact = null,//TODO
                LicenseInformation = null,//TODO
            };
        }
    }
}
