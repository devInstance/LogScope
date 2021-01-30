using DevInstance.LogScope.Formaters;
using DevInstance.LogScope.Logger;
using DevInstance.LogScope.Providers.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope
{
    public static class ScopeLogFactory
    {
        public static IScopeManager Create(LogLevel level, ILogProvider provider, IScopeFormater formater)
        {
            return new BaseScopeManager(level, provider, formater);
        }

        public static IScopeManager CreateWithDefaultFormater(LogLevel level, ILogProvider provider, bool showTimestamp)
        {
            return new BaseScopeManager(level, provider, new DefaultFormater(showTimestamp));
        }

        public static IScopeManager CreateConsoleLogger(LogLevel level, bool showTimestamp)
        {
            return CreateWithDefaultFormater(level, new ConsoleLogProvider(), showTimestamp);
        }
    }
}
