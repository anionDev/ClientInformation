namespace ClientInformation.Library.Core.Services
{
    public interface IClientInformationBackendService
    {
        public Model.ClientInformationBackendRecord GetClientInformationRecord(string ipAddress);
    }
}
