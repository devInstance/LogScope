using DevInstance.LogScope;
using DevInstance.LogScope.Extensions.MicrosoftLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicrosoftLoggerSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //var factory = host.Services.GetRequiredService<ILoggerFactory>();
            //logger.LogInformation("Host created.");
            
            host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    {
                        services.AddHostedService<SampleLoggingHostedService>();
                        services.AddMicrosoftScopeLogging(LogLevel.TRACE, "test");
                    }
                );
        }
    }
}
