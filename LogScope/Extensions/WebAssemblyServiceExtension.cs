using DevInstance.LogScope.Formaters;
using DevInstance.LogScope.Logger;
using DevInstance.LogScope.Providers.Console;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope.Extensions
{
    public static class WebAssemblyServiceExtension
    {
        public static IServiceCollection AddLoggingManager(this IServiceCollection col, IScopeManager manager)
        {
            return col.AddSingleton(manager);
        }

        public static IServiceCollection AddConsoleLogging(this IServiceCollection col, LogLevel level, bool showTimestamp)
        {
            return col.AddSingleton<IScopeManager>(
                new BaseScopeManager(
                    level, 
                    new ConsoleLogProvider(),
                    new DefaultFormater(showTimestamp)
                    )
                );
        }
    }
}
