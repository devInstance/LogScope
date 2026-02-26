using System;
using DevInstance.LogScope;
using DevInstance.LogScope.Extensions.SerilogLogger;
using DevInstance.LogScope.Formatters;
using Serilog;

namespace SerilogSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();

            Console.WriteLine(" ======== Serilog + LogScope ========");
            var manager = SerilogLogProviderFactory.CreateManager(
                LogLevel.TRACE,
                Log.Logger,
                new DefaultFormattersOptions
                {
                    ShowTimestamp = true,
                    ShowThreadNumber = true
                });

            var log = manager.CreateLogger("SerilogSample");

            log.I("Starting sample application");

            using (var scope = log.DebugScope())
            {
                scope.I("Inside a scope");
                scope.D("Debug message inside scope");

                using (var inner = scope.DebugScope("InnerScope"))
                {
                    inner.T("Trace message in inner scope");
                    inner.W("Warning in inner scope");
                }
            }

            log.E("Sample error message");
            log.I("Sample complete");

            Log.CloseAndFlush();
        }
    }
}
