using DevInstance.LogScope.Formatters;
using DevInstance.LogScope.Logger;
using DevInstance.LogScope.Providers.Console;

namespace DevInstance.LogScope
{
    /// <summary>
    /// Factory class to instantiate a scope manager.
    /// <example>
    /// Here is the example how to create console default logger:
    /// <code>
    ///     var manager = DefaultScopeLogFactory.Create(LogLevel.DEBUG, myprovider, myformatter);
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// Another example how to create custom provider and formatter:
    /// <code>
    ///     var manager = DefaultScopeLogFactory.CreateConsoleLogger(LogLevel.DEBUG);
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public static class DefaultScopeLogFactory
    {
        public static IScopeManager Create(ILogProvider provider, IScopeFormatter formater)
        {
            return new DefaultScopeManager(LogLevel.TRACE, provider, formater);
        }

        public static IScopeManager Create(LogLevel level, ILogProvider provider, IScopeFormatter formater)
        {
            return new DefaultScopeManager(level, provider, formater);
        }

        public static IScopeManager CreateWithDefaultFormatter(LogLevel level, ILogProvider provider, DefaultFormattersOptions options)
        {
            return new DefaultScopeManager(level, provider, new DefaultFormatter(options));
        }

        public static IScopeManager CreateConsoleLogger(LogLevel level, DefaultFormattersOptions options)
        {
            return CreateWithDefaultFormatter(level, new ConsoleLogProvider(), options);
        }

        public static IScopeManager CreateConsoleLogger(LogLevel level)
        {
            return CreateConsoleLogger(level, null);
        }
    }
}
