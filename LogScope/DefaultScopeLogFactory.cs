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
        /// <summary>
        /// Creates a scope manager with a custom provider and formatter using TRACE as the default log level.
        /// </summary>
        /// <param name="provider">The log provider implementation.</param>
        /// <param name="formater">The scope formatter implementation.</param>
        /// <returns>A new <see cref="IScopeManager"/> instance.</returns>
        public static IScopeManager Create(ILogProvider provider, IScopeFormatter formater)
        {
            return new DefaultScopeManager(LogLevel.TRACE, provider, formater);
        }

        /// <summary>
        /// Creates a scope manager with a custom provider, formatter, and log level.
        /// </summary>
        /// <param name="level">The minimum log level for messages to be written.</param>
        /// <param name="provider">The log provider implementation.</param>
        /// <param name="formater">The scope formatter implementation.</param>
        /// <returns>A new <see cref="IScopeManager"/> instance.</returns>
        public static IScopeManager Create(LogLevel level, ILogProvider provider, IScopeFormatter formater)
        {
            return new DefaultScopeManager(level, provider, formater);
        }

        /// <summary>
        /// Creates a scope manager with a custom provider and the default formatter with specified options.
        /// </summary>
        /// <param name="level">The minimum log level for messages to be written.</param>
        /// <param name="provider">The log provider implementation.</param>
        /// <param name="options">Options for customizing the default formatter output.</param>
        /// <returns>A new <see cref="IScopeManager"/> instance.</returns>
        public static IScopeManager CreateWithDefaultFormatter(LogLevel level, ILogProvider provider, DefaultFormattersOptions options)
        {
            return new DefaultScopeManager(level, provider, new DefaultFormatter(options));
        }

        /// <summary>
        /// Creates a console-based scope manager with custom formatter options.
        /// </summary>
        /// <param name="level">The minimum log level for messages to be written.</param>
        /// <param name="options">Options for customizing the formatter output, or null for defaults.</param>
        /// <returns>A new <see cref="IScopeManager"/> that outputs to the console.</returns>
        public static IScopeManager CreateConsoleLogger(LogLevel level, DefaultFormattersOptions options)
        {
            return CreateWithDefaultFormatter(level, new ConsoleLogProvider(), options);
        }

        /// <summary>
        /// Creates a console-based scope manager with default formatter options.
        /// </summary>
        /// <param name="level">The minimum log level for messages to be written.</param>
        /// <returns>A new <see cref="IScopeManager"/> that outputs to the console.</returns>
        public static IScopeManager CreateConsoleLogger(LogLevel level)
        {
            return CreateConsoleLogger(level, null);
        }
    }
}
