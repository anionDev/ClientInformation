using GRYLibrary.Core.Logging.GeneralPurposeLogger;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GRYLibrary.Core.APIServer.Utilities;

namespace ClientInformationBackend.Core.Miscellaneous
{
    public class HealthCheck : IHealthCheck
    {
        private readonly IGeneralLogger _Logger;
        public HealthCheck(IGeneralLogger logger)
        {
            this._Logger = logger;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            this._Logger.Log("Start calculating health-status", Microsoft.Extensions.Logging.LogLevel.Debug);
            return await Tools.CheckHealthAsync(this._Logger, () =>
            {
                this._Logger.Log("Calculate health-status...", Microsoft.Extensions.Logging.LogLevel.Debug);
                IList<string> messages = new List<string>();
                HealthStatus result = HealthStatus.Healthy;

                return (result, messages);
            }, context, cancellationToken);
        }
    }
}
