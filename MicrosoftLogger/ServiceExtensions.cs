using DevInstance.LogScope.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevInstance.LogScope.Extensions.MicrosoftLogger
{
    /// <summary>
    /// This class contains a collection of methods to support dependency injection (DI) software design pattern
    /// , which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies.
    /// </summary>
    public static class ServiceExtensions
    {
        internal class MicrosoftLoggerSettings
        {
            public DefaultFormattersOptions options;
        }

        /// <summary>
        /// Creates and adds a microsoft logger and scope manager to the service collection.
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns></returns>
        public static IServiceCollection AddMicrosoftScopeLogging(this IServiceCollection col, LogLevel level, string categoryName)
        {
            var factory = LoggerFactory.Create(logging =>
            {
                logging.Configure(options =>
                {
                    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                                        | ActivityTrackingOptions.TraceId
                                                        | ActivityTrackingOptions.ParentId
                                                        | ActivityTrackingOptions.Baggage
                                                        | ActivityTrackingOptions.Tags;
                }).AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                }
                ).SetMinimumLevel(MicrosoftLogProvider.ConvertLevel(level));
            });

            return col.AddMicrosoftScopeLogging(factory.CreateLogger(categoryName), level, null);
        }

        /// <summary>
        /// Creates and adds a manager with existing instance of ILogger to the service collection.
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="logger">instance of ILogger</param>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <param name="o">
        ///     Options for the default formatter. See <see cref="DefaultFormattersOptions"/>
        /// </param>
        /// <returns></returns>
        public static IServiceCollection AddMicrosoftScopeLogging(this IServiceCollection col, ILogger logger, LogLevel level, DefaultFormattersOptions o)
        {
            var factory = MicrosoftLogProviderFactory.CreateManager(level, logger, o);
            return col.AddSingleton<IScopeManager>(factory);
        }
    }
}
