namespace ClientInformationBackend.Core.Services
{
    public interface IClientInformationBackendService
    {
        public Model.ClientInformationBackendRecord GetClientInformationBackend(string ipAddress);
    }
}
