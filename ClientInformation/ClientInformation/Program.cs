using ClientInformation.Core.Constants;
using GRYLibrary.Core.Misc;
using Microsoft.Extensions.DependencyInjection;
using GUtilities = GRYLibrary.Core.Misc.Utilities;
using ClientInformation.Core.Configuration;
using GRYLibrary.Core.APIServer.CommonRoutes;
using GRYLibrary.Core.APIServer.ConcreteEnvironments;
using System.Collections.Generic;
using GRYLibrary.Core.APIServer.Utilities;
using Microsoft.Extensions.Logging;
using GRYLibrary.Core.Logging.GRYLogger;
using GRYLibrary.Core.APIServer.Mid.DLog;
using GRYLibrary.Core.APIServer.MidT.Exception;
using GRYLibrary.Core.APIServer.Mid.Ex;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Metrics;
using GRYLibrary.Core.APIServer.Mid.PreDAPIK;
using ClientInformation.Core.Miscellaneous;
using GRYLibrary.Core.APIServer.Services.Interfaces;
using GRYLibrary.Core.APIServer.Services.TS;
using GRYLibrary.Core.APIServer.MaintenanceRoutes;
using ClientInformation.Core.Services;

namespace ClientInformation.Core
{
    internal class Program
    {
        private static IGRYLog _Logger;
        internal static int Main(string[] commandlineArguments)
        {
            return Tools.RunAPIServer<CodeUnitSpecificCommandlineParameter, CodeUnitSpecificConstants, CodeUnitSpecificConfiguration>(GeneralConstants.CodeUnitName, GeneralConstants.CodeUnitDescription, Version3.Parse(GeneralConstants.CodeUnitVersion), GetEnvironmentTargetType(), GUtilities.GetExecutionMode(commandlineArguments), commandlineArguments, (apiServerConfiguration) =>
            {
                apiServerConfiguration.SetInitialzationInformationAction = (initializationInformation) =>
                {
                    string domain = Tools.GetDefaultDomainValue(GeneralConstants.CodeUnitName);
                    uint defaultUpdateIntervalInMinutes = 30;//Load current price 2 time per hour
                    uint updateIntervalInMinutesForHistoricalData = 1;//Load historical prices 1 time per hour. This value can be updated for example to 2, 3, 6, 10, 20, 30, 60. Important. This value has 2 constraints: 1.: 0 < value <=60. 2.: 60/value must be an integer without decimal places.

                    initializationInformation.ApplicationConstants.CommonRoutesHostInformation = new HostCommonRoutes();
                    initializationInformation.ApplicationConstants.HostMaintenanceInformation = new HostMaintenanceRoutes();
                    initializationInformation.ApplicationConstants.LoggingMiddleware = typeof(DRequestLoggingMiddleware);
                    initializationInformation.ApplicationConstants.ExceptionManagerMiddleware = typeof(DefaultExceptionHandlerMiddleware);
                    initializationInformation.ApplicationConstants.AuthorizationMiddleware = typeof(PreDAPIKValidatorMiddleware);
                    initializationInformation.InitialApplicationConfiguration.ApplicationSpecificConfiguration.CommonRoutesInformation = new CommonRoutesInformation()
                    {
                        ContactLink = $"https://information.{domain}/Products/{GeneralConstants.CodeUnitName}/Contact",
                        LicenseLink = $"https://information.{domain}/Products/{GeneralConstants.CodeUnitName}/License",
                        TermsOfServiceLink = $"https://information.{domain}/Products/{GeneralConstants.CodeUnitName}/TermsOfService"
                    };
                    initializationInformation.InitialApplicationConfiguration.ApplicationSpecificConfiguration.MaintenanceRoutesInformation = new MaintenanceRoutesInformation();
                    bool verbose = initializationInformation.ApplicationConstants.Environment is not Productive;
                    initializationInformation.InitialApplicationConfiguration.ApplicationSpecificConfiguration.RequestLoggingConfiguration = new DRequestLoggingConfiguration()
                    {
                        NotLoggedRoutes = new HashSet<string>() {
                            "^/favicon.ico",
                            "^/API/Other/Resources/APISpecification/.*",
                            "^/API/Other/Maintenance/Metrics",
                        },
                    };
                    initializationInformation.InitialApplicationConfiguration.ApplicationSpecificConfiguration.ConfigurationForExceptionManagerMiddleware = new ExceptionManagerConfiguration();
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.HostAPISpecificationForInNonDevelopmentEnvironment = true;
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.Protocol = initializationInformation.ApplicationConstants.ExecutionMode.Accept(new GetProcolVisitor(domain));
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.SetDomainAndPublichUrlToDefault(domain);
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.DevelopmentCertificatePasswordHex = GeneralConstants.DevelopmentCertificatePasswordHex;
                    initializationInformation.InitialApplicationConfiguration.ServerConfiguration.DevelopmentCertificatePFXHex = GeneralConstants.DevelopmentCertificatePFXHex;
                };
                apiServerConfiguration.SetFunctionalInformationAction = (functionalInformation) =>
                {
                    _Logger = functionalInformation.Logger;
                    TimeService timeService = new TimeService();

                    functionalInformation.WebApplicationBuilder.Services.AddHealthChecks().AddCheck<HealthCheck>(nameof(HealthCheck));
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton(functionalInformation.PersistedAPIServerConfiguration.ApplicationSpecificConfiguration.ConfigurationForExceptionManagerMiddleware);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton(functionalInformation.PersistedAPIServerConfiguration.ApplicationSpecificConfiguration.RequestLoggingConfiguration);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton(functionalInformation.PersistedAPIServerConfiguration.ApplicationSpecificConfiguration.CommonRoutesInformation);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton(functionalInformation.PersistedAPIServerConfiguration.ApplicationSpecificConfiguration.MaintenanceRoutesInformation);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<ITimeService>(timeService);
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<IMetricsService, MetricsService>();
                    functionalInformation.WebApplicationBuilder.Services.AddSingleton<IClientInformationService, ClientInformationService>();
                    functionalInformation.WebApplicationBuilder.Services.AddOpenTelemetry().WithMetrics(builder =>
                      {
                          builder.AddMeter(CodeUnitSpecificConstants.MetricAmountOfDataSetsName);
                          builder.AddPrometheusExporter();
                      });
                };
                apiServerConfiguration.ConfigureWebApplication = (functionalInformationForWebApplication) =>
                {
                    bool metricsServiceEnabled = true;
                    bool currentPriceUpdateServiceEnabled = true;
                    bool historicalPriceUpdateServiceEnabled = true;
                    functionalInformationForWebApplication.PreRun = () =>
                    {
                        _Logger.Log($"Start services...", LogLevel.Information);
                        if (metricsServiceEnabled)
                        {
                            functionalInformationForWebApplication.WebApplication.Services.GetService<IMetricsService>().StartAsync();
                        }
                    };
                    functionalInformationForWebApplication.PostRun = () =>
                    {
                        _Logger.Log($"Stop services...", LogLevel.Information);
                        if (metricsServiceEnabled)
                        {
                            functionalInformationForWebApplication.WebApplication.Services.GetService<IMetricsService>().Stop().Wait();
                        }
                    };
                    functionalInformationForWebApplication.WebApplication.MapHealthChecks(GRYLibrary.Core.APIServer.Utilities.Constants.UsualHealthCheckEndpoint);
                    functionalInformationForWebApplication.WebApplication.UseOpenTelemetryPrometheusScrapingEndpoint(GRYLibrary.Core.APIServer.Utilities.Constants.UsualMetricsEndpoint);
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
