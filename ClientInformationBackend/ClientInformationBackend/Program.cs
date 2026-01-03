using ClientInformationBackend.Core.Configuration;
using ClientInformationBackend.Core.Constants;
using ClientInformationBackend.Core.Miscellaneous;
using ClientInformationBackend.Core.Services;
using GRYLibrary.Core.APIServer.CommonRoutes;
using GRYLibrary.Core.APIServer.ConcreteEnvironments;
using GRYLibrary.Core.APIServer.MaintenanceRoutes;
using GRYLibrary.Core.APIServer.Mid.Ex;
using GRYLibrary.Core.APIServer.Mid.M05DLog;
using GRYLibrary.Core.APIServer.Mid.PreDAPIK;
using GRYLibrary.Core.APIServer.MidT.Exception;
using GRYLibrary.Core.APIServer.Services.Init;
using GRYLibrary.Core.APIServer.Services.Interfaces;
using GRYLibrary.Core.APIServer.Services.OtherServices;
using GRYLibrary.Core.APIServer.Settings.Configuration;
using GRYLibrary.Core.APIServer.Utilities;
using GRYLibrary.Core.Logging.GRYLogger;
using GRYLibrary.Core.Misc;
using GRYLibrary.Core.Misc.ConsoleApplication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using GUtilities = GRYLibrary.Core.Misc.Utilities;

namespace ClientInformationBackend.Core
{
    internal class Program
    {
        private static IGRYLog _Logger;
        internal static int Main(string[] commandlineArguments)
        {
            return Tools.RunAPIServer<CodeUnitSpecificCommandlineParameter, CodeUnitSpecificConstants, CodeUnitSpecificConfiguration>(GeneralConstants.CodeUnitName, GeneralConstants.CodeUnitDescription, Version3.Parse(GeneralConstants.CodeUnitVersion), GetEnvironmentTargetType(), GUtilities.GetExecutionMode(commandlineArguments), commandlineArguments, null, (apiServerConfiguration) =>
            {
                apiServerConfiguration.SetInitialzationInformationAction = (initializationInformation) =>
                {
                    string domain = string.IsNullOrWhiteSpace(initializationInformation.CommandlineParameter.InitialDomain) ? Tools.GetDefaultDomainValue(GeneralConstants.CodeUnitName) : initializationInformation.CommandlineParameter.InitialDomain;

                    initializationInformation.ApplicationConstants.CommonRoutesHostInformation = new HostCommonRoutes();
                    initializationInformation.ApplicationConstants.HostMaintenanceInformation = new HostMaintenanceRoutes();
                    initializationInformation.ApplicationConstants.LoggingMiddleware = typeof(DRequestLoggingMiddleware);
                    initializationInformation.ApplicationConstants.ExceptionManagerMiddleware = typeof(DefaultExceptionHandlerMiddleware);
                    initializationInformation.InitialApplicationConfiguration.ApplicationSpecificConfiguration.CommonRoutesInformation = new CommonRoutesInformation()
                    {
                        ContactLink = $"https://information.{domain}/Products/{GeneralConstants.CodeUnitName}/Contact",
                        LicenseLink = $"https://information.{domain}/Products/{GeneralConstants.CodeUnitName}/License",
                        TermsOfServiceLink = $"https://information.{domain}/Products/{GeneralConstants.CodeUnitName}/TermsOfService"
                    };
                    bool verbose = initializationInformation.ApplicationConstants.Environment is not Productive;
                    initializationInformation.InitialApplicationConfiguration.ApplicationSpecificConfiguration.RequestLoggingConfiguration = new DRequestLoggingConfiguration()
                    {
                        NotLoggedRoutes = new HashSet<string>() {
                            @$"^/API/Other/Resources/APISpecification/*",
                            @$"^/API/Other/Maintenance/Metrics$",
                            @$"^/API/Other/Maintenance/HealthCheck$",
                        },
                    };
                    initializationInformation.InitialApplicationConfiguration.ApplicationSpecificConfiguration.MaintenanceRoutesInformation = new MaintenanceRoutesInformation()
                    {
                        EnableEndpointAvailabilityCheck = initializationInformation.CommandlineParameter.InitialEnableEndpointAvailabilityCheckValue,
                        EnableEndpointInitializationState = initializationInformation.CommandlineParameter.InitialEnableEndpointInitializationStateValue,
                        EnableEndpointCurrentVersion = initializationInformation.CommandlineParameter.InitialEnableEndpointCurrentVersionValue,
                        EnableEndpointShowAllEndpoints = initializationInformation.CommandlineParameter.InitialEnableEndpointShowAllEndpointsValue,
                        EnableEndpointHealthCheck = initializationInformation.CommandlineParameter.InitialEnableEndpointHealthCheckValue,
                        EnableEndpointMetrics = initializationInformation.CommandlineParameter.InitialEnableEndpointMetricsValue,
                    };
                    initializationInformation.InitialApplicationConfiguration.ApplicationSpecificConfiguration.ConfigurationForExceptionManagerMiddleware = new ExceptionManagerConfiguration();
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.HostAPISpecificationForInNonDevelopmentEnvironment = true;
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.Protocol = new HTTP();
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.SetDomainAndPublichUrlToDefault(domain);
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.DevelopmentCertificatePasswordHex = GeneralConstants.DevelopmentCertificatePasswordHex;
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.DevelopmentCertificatePFXHex = GeneralConstants.DevelopmentCertificatePFXHex;
                };
                apiServerConfiguration.SetFunctionalInformationAction = (functionalInformation) =>
                {
                    _Logger = functionalInformation.Logger;
                    TimeService timeService = new TimeService();

                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<IHealthCheck, HealthCheck>();
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton(functionalInformation.PersistedAPIServerConfiguration.ApplicationSpecificConfiguration.ConfigurationForExceptionManagerMiddleware);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton(functionalInformation.PersistedAPIServerConfiguration.ApplicationSpecificConfiguration.RequestLoggingConfiguration);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton(functionalInformation.PersistedAPIServerConfiguration.ApplicationSpecificConfiguration.CommonRoutesInformation);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton(functionalInformation.PersistedAPIServerConfiguration.ApplicationSpecificConfiguration.MaintenanceRoutesInformation);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<ITimeService>(timeService);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<IMetricsService, MetricsService>();
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<IInitializationService<CodeUnitSpecificCommandlineParameter>, InitializationService>();
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<IInitializationService>(sp => sp.GetRequiredService<IInitializationService<CodeUnitSpecificCommandlineParameter>>());
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<IClientInformationBackendService, ClientInformationBackendService>();
                };
                apiServerConfiguration.ConfigureWebApplication = (functionalInformationForWebApplication) =>
                {
                    bool runServices = functionalInformationForWebApplication.InitializationInformation.CommandlineParameter.RealRun;
                    functionalInformationForWebApplication.PreRun = () =>
                    {
                        if (runServices)
                        {
                            _Logger.Log($"Start services...", LogLevel.Information);
                            functionalInformationForWebApplication.WebApplication.Services.GetService<IMetricsService>().StartAsync();
                        }
                    };
                    functionalInformationForWebApplication.PostRun = () =>
                    {
                        if (runServices)
                        {
                            _Logger.Log($"Stop services...", LogLevel.Information);
                            functionalInformationForWebApplication.WebApplication.Services.GetService<IMetricsService>().Stop().Wait();
                        }
                    };
                };
            });
        }

        private static GRYEnvironment GetEnvironmentTargetType()
        {
#if Development
            return Development.Instance;
#elif QualityCheck
            return QualityCheck.Instance;
#elif Productive
            return Productive.Instance;
#else
            throw new System.Collections.Generic.KeyNotFoundException("Unknown environmenttargettype.");
#endif
        }
    }
}
