using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DevInstance.LogScope.Extensions.MicrosoftLogger")]
[assembly: InternalsVisibleTo("LogScope.Tests")]
[assembly: InternalsVisibleTo("DevInstance.LogScope.NET")]
namespace DevInstance.LogScope.Logger
{
    internal struct LContext
    {
        public ILogProvider Provider { get; internal set; }
        public IScopeManager Manager { get; internal set; }
        public IScopeFormatter Formatter { get; internal set; }
    }

    internal class DefaultScopeManager : IScopeManager
    {
        public LogLevel BaseLevel { get; private set; }
        public ILogProvider Provider { get; }
        public IScopeFormatter Formater { get; }

        private LContext context;

        public DefaultScopeManager(LogLevel level, ILogProvider provider, IScopeFormatter formater)
        {
            if (provider == null || formater == null)
            {
                throw new ArgumentNullException();
            }

            BaseLevel = level;
            Provider = provider;
            Formater = formater;

            context = new LContext { Formatter = formater, Manager = this, Provider = provider };
        }

        public IScopeLog CreateLogger([CallerMemberName] string scope = null)
        {
            return new DefaultScopeLog(context, BaseLevel, BaseLevel, null, scope, false);
        }

        public IScopeLog CreateLogger(string scope, LogLevel levelOverride)
        {
            return new DefaultScopeLog(context, levelOverride, levelOverride, null, scope, false);
        }
    }
}
