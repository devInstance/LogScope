using DevInstance.LogScope;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicrosoftLoggerSampleApp
{
    internal sealed class SampleLoggingHostedService : IHostedService
    {
        IScopeLog Log { get; }

        public SampleLoggingHostedService(IScopeManager manager)
        {
            Log = manager.CreateLogger(this);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using(var l = Log.TraceScope())
            {
                l.T("Trace line");
                l.D("Debug line");
                l.I("Info line");
                l.W("Warning line");
                l.E("Error line");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}
