using DevInstance.LogScope;
using DevInstance.LogScope.Extensions.MicrosoftLogger;
using DevInstance.LogScope.Formatters;
using InternalTestApp;
using Microsoft.Extensions.Logging;
using LogLevel = DevInstance.LogScope.LogLevel;

//var manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.INFO, new DefaultFormattersOptions { ShowTimestamp = true, ShowThreadNumber = true, ShowId = true });
var manager = MicrosoftLogProviderFactory.CreateManager(LogLevel.INFO, CreateMicrosoftLogger().CreateLogger("test"), null);

var perfTest = new PerfComparisonTest();
perfTest.Test(manager);

Console.ReadKey();

static ILoggerFactory CreateMicrosoftLogger()
{
    return LoggerFactory.Create(logging =>
    {
        logging.Configure(options =>
        {
            options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                                | ActivityTrackingOptions.TraceId
                                                | ActivityTrackingOptions.ParentId
                                                | ActivityTrackingOptions.Baggage
                                                | ActivityTrackingOptions.Tags;
        }).AddSimpleConsole(options =>
        {
            options.IncludeScopes = false;
        }
        ).SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    });
}