using DevInstance.LogScope.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevInstance.LogScope.Extensions.MicrosoftLogger
{
    public static class ServiceExtensions
    {
        internal class MicrosoftLoggerSettings
        {
            public DefaultFormattersOptions options;
        }

        public static IServiceCollection AddMicrosoftScopeLogging(this IServiceCollection col)
        {
            return col.AddMicrosoftScopeLogging(null);
        }

        public static IServiceCollection AddMicrosoftScopeLogging(this IServiceCollection col, DefaultFormattersOptions o)
        {
            var settings = new MicrosoftLoggerSettings
            {
                options = o
            };
            col.AddSingleton(settings);
            return col.AddSingleton<IScopeManager, LoggingExtensionService>();
        }
    }
}
