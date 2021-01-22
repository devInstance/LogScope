using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope.Extensions
{
    public static class WebAssemblyServiceExtension
    {
        public static IServiceCollection AddConsoleLogging(this IServiceCollection col, LogLevel level)
        {
            return col.AddSingleton<ILogProvider>(new ConsoleLogProvider(level));
        }

        public static IServiceCollection AddLoggingProvider(this IServiceCollection col, ILogProvider provider)
        {
            return col.AddSingleton(provider);
        }
    }
}
