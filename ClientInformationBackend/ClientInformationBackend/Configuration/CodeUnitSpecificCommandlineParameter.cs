using CommandLine;
using GRYLibrary.Core.APIServer.Verbs;

namespace ClientInformationBackend.Core.Configuration
{
    public class CodeUnitSpecificCommandlineParameter : RunServer
    {

        [Option(nameof(InitialDomain), Required = false)]
        public string? InitialDomain { get; set; }

        [Option(nameof(InitialEnableEndpointAvailabilityCheckValue), Required = false, Default = true)]
        public bool InitialEnableEndpointAvailabilityCheckValue { get; set; }

        [Option(nameof(InitialEnableEndpointInitializationStateValue), Required = false, Default = false)]
        public bool InitialEnableEndpointInitializationStateValue { get; set; }

        [Option(nameof(InitialEnableEndpointCurrentVersionValue), Required = false, Default = false)]
        public bool InitialEnableEndpointCurrentVersionValue { get; set; }

        [Option(nameof(InitialEnableEndpointShowAllEndpointsValue), Required = false, Default = false)]
        public bool InitialEnableEndpointShowAllEndpointsValue { get; set; }

        [Option(nameof(InitialEnableEndpointHealthCheckValue), Required = false, Default = true)]
        public bool InitialEnableEndpointHealthCheckValue { get; set; }

        [Option(nameof(InitialEnableEndpointMetricsValue), Required = false, Default = false)]
        public bool InitialEnableEndpointMetricsValue { get; set; }
    }
}
