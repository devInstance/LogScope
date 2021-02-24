using DevInstance.LogScope.Formaters;
using DevInstance.LogScope.Logger;
using DevInstance.LogScope.Providers.Console;

namespace DevInstance.LogScope
{
    /// <summary>
    /// Factory class to instantiate a scope manager.
    /// <example>
    /// Here is the example how to create console default logger:
    /// <code>
    ///     var manager = ScopeLogFactory.Create(LogLevel.DEBUG, myprovider, myformatter);
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// Another example how to create custom provider and formatter:
    /// <code>
    ///     var manager = ScopeLogFactory.CreateConsoleLogger(LogLevel.DEBUG);
    /// }
    /// </code>
    /// </example>
    /// </summary>
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
