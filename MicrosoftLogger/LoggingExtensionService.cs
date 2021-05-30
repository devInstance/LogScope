using DevInstance.LogScope.Formatters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using static DevInstance.LogScope.Extensions.MicrosoftLogger.ServiceExtensions;

namespace DevInstance.LogScope.Extensions.MicrosoftLogger
{
    internal class LoggingExtensionService : IScopeManager
    {
        ILoggerFactory logFactory;
        MicrosoftLoggerSettings logSettings;
        IScopeFormatter formatter;
        public LoggingExtensionService(ILoggerFactory factory, MicrosoftLoggerSettings settings) 
        {
            logFactory = factory;
            logSettings = settings;
            formatter = new DefaultFormatter(settings.options);
        }

        public LogLevel BaseLevel => throw new NotImplementedException();

        public IScopeLog CreateLogger(string scope)
        {
            return new ScopeLog(logFactory, formatter, LogLevel.UNDEFINED, scope, false);
        }

        public IScopeLog CreateLogger(string scope, LogLevel levelOverride)
        {
            throw new NotSupportedException("Override is not supported by Microsoft Logger");
        }
    }
}
