using DevInstance.LogScope.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DevInstance.LogScope.Extensions.SerilogLogger
{
    /// <summary>
    /// This class contains a collection of methods to support dependency injection (DI) software design pattern
    /// , which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Creates and adds a Serilog-based scope manager to the service collection
        /// using the global <see cref="Log.Logger"/>.
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <param name="o">
        ///     Options for the default formatter. See <see cref="DefaultFormattersOptions"/>
        /// </param>
        /// <returns></returns>
        public static IServiceCollection AddSerilogScopeLogging(this IServiceCollection col, LogLevel level, DefaultFormattersOptions o = null)
        {
            return col.AddSerilogScopeLogging(Log.Logger, level, o);
        }

        /// <summary>
        /// Creates and adds a scope manager with an existing instance of Serilog ILogger to the service collection.
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="logger">instance of Serilog ILogger</param>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <param name="o">
        ///     Options for the default formatter. See <see cref="DefaultFormattersOptions"/>
        /// </param>
        /// <returns></returns>
        public static IServiceCollection AddSerilogScopeLogging(this IServiceCollection col, ILogger logger, LogLevel level, DefaultFormattersOptions o = null)
        {
            var manager = SerilogLogProviderFactory.CreateManager(level, logger, o);
            return col.AddSingleton<IScopeManager>(manager);
        }
    }
}
