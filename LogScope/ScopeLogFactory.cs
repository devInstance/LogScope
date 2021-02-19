using DevInstance.LogScope.Formaters;
using DevInstance.LogScope.Logger;
using DevInstance.LogScope.Providers.Console;

namespace DevInstance.LogScope
{
    public static class ScopeLogFactory
    {
        public static IScopeManager Create(ILogProvider provider, IScopeFormater formater)
        {
            return new BaseScopeManager(LogLevel.TRACE, provider, formater);
        }

        public static IScopeManager Create(LogLevel level, ILogProvider provider, IScopeFormater formater)
        {
            return new BaseScopeManager(level, provider, formater);
        }

        public static IScopeManager CreateWithDefaultFormater(LogLevel level, ILogProvider provider, DefaultFormaterOptions options)
        {
            return new BaseScopeManager(level, provider, new DefaultFormater(options));
        }

        public static IScopeManager CreateConsoleLogger(LogLevel level, DefaultFormaterOptions options)
        {
            return CreateWithDefaultFormater(level, new ConsoleLogProvider(), options);
        }

        public static IScopeManager CreateConsoleLogger(LogLevel level)
        {
            return CreateConsoleLogger(level, null);
        }
    }
}
