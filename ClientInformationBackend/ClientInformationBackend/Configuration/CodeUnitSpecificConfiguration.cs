using GRYLibrary.Core.APIServer.CommonRoutes;
using GRYLibrary.Core.APIServer.MaintenanceRoutes;
using GRYLibrary.Core.APIServer.Mid.M05DLog;
using GRYLibrary.Core.APIServer.MidT.Exception;
using GRYLibrary.Core.APIServer.MidT.RLog;

namespace ClientInformationBackend.Core.Configuration
{
    public class CodeUnitSpecificConfiguration : ISupportRequestLoggingMiddleware, ISupportExceptionManagerMiddleware
    {
        public ICommonRoutesInformation CommonRoutesInformation { get; set; }
        public IMaintenanceRoutesInformation MaintenanceRoutesInformation { get; set; }
        public IDRequestLoggingConfiguration RequestLoggingConfiguration { get; set; }
        public IRequestLoggingConfiguration ConfigurationForLoggingMiddleware { get { return this.RequestLoggingConfiguration; } }
        public IExceptionManagerConfiguration ConfigurationForExceptionManagerMiddleware { get; set; }
        public string? LicenseInformation { get; set; }
        public string? ContactInformation { get; set; }
    }
}
