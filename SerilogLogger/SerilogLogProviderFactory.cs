using DevInstance.LogScope.Formatters;
using Serilog;

namespace DevInstance.LogScope.Extensions.SerilogLogger
{
    /// <summary>
    /// Factory class for creating scope managers that integrate with Serilog.
    /// </summary>
    public static class SerilogLogProviderFactory
    {
        /// <summary>
        /// Creates a scope manager with existing instance of Serilog ILogger.
        /// </summary>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <param name="logger">instance of Serilog ILogger</param>
        /// <param name="o">
        ///     Options for the default formatter. See <see cref="DefaultFormattersOptions"/>
        /// </param>
        /// <returns></returns>
        public static IScopeManager CreateManager(LogLevel level, ILogger logger, DefaultFormattersOptions o)
        {
            return DefaultScopeLogFactory.CreateWithDefaultFormatter(level, new SerilogLogProvider(logger), o);
        }
    }
}
