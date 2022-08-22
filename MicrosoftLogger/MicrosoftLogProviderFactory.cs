using DevInstance.LogScope.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.LogScope.Extensions.MicrosoftLogger
{
    public static class MicrosoftLogProviderFactory
    {
        /// <summary>
        /// Creates a scope manager with existing instance of ILogger.
        /// </summary>
        /// <param name="level">logging level. See <see cref="LogLevel"/></param>
        /// <param name="logger">instance of ILogger</param>
        /// <param name="o">
        ///     Options for the default formatter. See <see cref="DefaultFormattersOptions"/>
        /// </param>
        /// <returns></returns>
        public static IScopeManager CreateManager(LogLevel level, ILogger logger, DefaultFormattersOptions o)
        {
            return DefaultScopeLogFactory.CreateWithDefaultFormatter(level, new MicrosoftLogProvider(logger), o);
        }
    }
}
