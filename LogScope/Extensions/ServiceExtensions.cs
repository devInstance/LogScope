using DevInstance.LogScope.Formaters;
using DevInstance.LogScope.Logger;
using DevInstance.LogScope.Providers.Console;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope.Extensions
{
    /// <summary>
    /// This class contains a collection of methods to support dependency injection (DI) software design pattern
    /// , which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies.
    /// </summary>
    public static class ServiceExtensions
    {

        /// <summary>
        /// Adds already instantiated manager to the service collection. This method is useful 
        /// for adding manager with custom provider and formatter. <seealso cref="ScopeLogFactory"/>
        /// for more information how to create custom provider and formatter.
        /// <example>
        /// Here is the example how use it in ConfigureServices method in ASP.NET Core app:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        /// ...
        ///     var manager = ScopeLogFactory.Create(LogLevel.DEBUG, myprovider, myformatter);
        ///     services.AddLoggingManager(manager);
        /// ...
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="manager">instance of the log scope manager</param>
        /// <returns>A reference to this instance after the operation has completed</returns>
        public static IServiceCollection AddLoggingManager(this IServiceCollection col, IScopeManager manager)
        {
            return col.AddSingleton(manager);
        }
        /// <summary>
        /// Creates and adds console manager to the service collection.
        /// <example>
        /// Here is the example how use it in ConfigureServices method in ASP.NET Core app:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        /// ...
        ///     services.AddConsoleLogging(LogLevel.DEBUG);
        /// ...
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <returns>A reference to this instance after the operation has completed</returns>
        public static IServiceCollection AddConsoleLogging(this IServiceCollection col, LogLevel level)
        {
            return col.AddConsoleLogging(level, null);
        }
        /// <summary>
        /// Creates and adds console manager to the service collection.
        /// <example>
        /// Here is the example how use it in ConfigureServices method in ASP.NET Core app:
        ///     <code>
        ///     public void ConfigureServices(IServiceCollection services)
        ///     {
        ///         ...
        ///         services.AddConsoleLogging(LogLevel.DEBUG, new DefaultFormaterOptions { ShowTimestamp = true, ShowThreadNumber = true });
        ///         ...
        ///     }
        ///     </code>
        ///     </example>
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <param name="options">
        ///     Options for the default formatter. See <see cref="DefaultFormaterOptions"/>
        /// </param>
        /// <returns>A reference to this instance after the operation has completed</returns>
        public static IServiceCollection AddConsoleLogging(this IServiceCollection col, LogLevel level, DefaultFormaterOptions options)
        {
            return col.AddSingleton<IScopeManager>(
                new BaseScopeManager(
                    level,
                    new ConsoleLogProvider(),
                    new DefaultFormater(options)
                    )
                );
        }
    }
}
