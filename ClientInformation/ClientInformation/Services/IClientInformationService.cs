namespace ClientInformation.Core.Services
{
    public interface IClientInformationService
    {
        public Model.ClientInformationRecord GetClientInformation(string ipAddress);
    }
}
