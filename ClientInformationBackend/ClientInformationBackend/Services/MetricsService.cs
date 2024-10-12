using ClientInformationBackend.Core.Constants;
using GRYLibrary.Core.APIServer.BaseServices;
using GRYLibrary.Core.APIServer.Settings;
using GRYLibrary.Core.Logging.GRYLogger;
using System.Diagnostics.Metrics;
using System;
using System.Threading;

namespace ClientInformationBackend.Core.Services
{
    public class MetricsService : IteratingBackgroundService, IMetricsService
    {
        private readonly Meter _AmountOfDataSetsMeter;
        public Counter<long> MetricAmountOfDataSets { get; private set; }
        public TimeSpan AdditionalDelay { get; set; } = TimeSpan.FromSeconds(20);
        public MetricsService(IApplicationConstants<CodeUnitSpecificConstants> constants, IGRYLog logger) : base(constants.ExecutionMode, logger)
        {
            this._AmountOfDataSetsMeter = new Meter(CodeUnitSpecificConstants.MetricAmountOfDataSetsName);
            this.MetricAmountOfDataSets = this._AmountOfDataSetsMeter.CreateCounter<long>(CodeUnitSpecificConstants.MetricAmountOfDataSetsName);
            this.Enabled = true;
        }

        public void CalculateHealthAndMetrics()
        {
            try
            {
                this.MetricAmountOfDataSets.Add(5);//TODO set actual value
            }
            catch (Exception exception)
            {
                this._Logger.Log("Error while calculating metrics", exception);
            }
        }

        protected override void Run()
        {
            Thread.Sleep(this.AdditionalDelay);
            this.CalculateHealthAndMetrics();
        }
    }
}
