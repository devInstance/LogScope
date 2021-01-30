using DevInstance.LogScope.Logger;
using DevInstance.LogScope.Providers.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope
{
    public static class ScopeLogFactory
    {
        public static IScopeManager Create(LogLevel level, ILogProvider provider)
        {
            return new BaseScopeManager(level, provider);
        }

        public static IScopeManager CreateConsoleLogger(LogLevel level)
        {
            return Create(level, new ConsoleLogProvider());
        }
    }
}
