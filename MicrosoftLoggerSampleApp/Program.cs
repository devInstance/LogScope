using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace MicrosoftLoggerSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var factory = host.Services.GetRequiredService<ILoggerFactory>();
            logger.LogInformation("Host created.");

            host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning));
        }
    }
}
