using DevInstance.LogScope.Formatters;
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
        /// for adding manager with custom provider and formatter. <seealso cref="DefaultScopeLogFactory"/>
        /// for more information how to create custom provider and formatter.
        /// <example>
        /// Here is the example how use it in ConfigureServices method in ASP.NET Core app:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        /// ...
        ///     var manager = ScopeLogFactory.Create(LogLevel.DEBUG, myprovider, myformatter);
        ///     services.AddScopeLogging(manager);
        /// ...
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="manager">instance of the log scope manager</param>
        /// <returns>A reference to this instance after the operation has completed</returns>
        public static IServiceCollection AddScopeLogging(this IServiceCollection col, IScopeManager manager)
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
        ///     services.AddConsoleScopeLogging(LogLevel.DEBUG);
        /// ...
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <returns>A reference to this instance after the operation has completed</returns>
        public static IServiceCollection AddConsoleScopeLogging(this IServiceCollection col, LogLevel level)
        {
            return col.AddConsoleScopeLogging(level, null);
        }
        /// <summary>
        /// Creates and adds console manager to the service collection.
        /// <example>
        /// Here is the example how use it in ConfigureServices method in ASP.NET Core app:
        ///     <code>
        ///     public void ConfigureServices(IServiceCollection services)
        ///     {
        ///         ...
        ///         services.AddConsoleScopeLogging(LogLevel.DEBUG, new DefaultFormaterOptions { ShowTimestamp = true, ShowThreadNumber = true });
        ///         ...
        ///     }
        ///     </code>
        ///     </example>
        /// </summary>
        /// <param name="col">collection of services</param>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <param name="options">
        ///     Options for the default formatter. See <see cref="DefaultFormattersOptions"/>
        /// </param>
        /// <returns>A reference to this instance after the operation has completed</returns>
        public static IServiceCollection AddConsoleScopeLogging(this IServiceCollection col, LogLevel level, DefaultFormattersOptions options)
        {
            return col.AddSingleton<IScopeManager>(
                new DefaultScopeManager(
                    level,
                    new ConsoleLogProvider(),
                    new DefaultFormatter(options)
                    )
                );
        }
    }
}
