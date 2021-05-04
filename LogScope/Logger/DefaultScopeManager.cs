using DevInstance.LogScope.Formatters;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DevInstance.LogScope.Extensions.MicrosoftLogger")]
[assembly: InternalsVisibleTo("LogScope.Tests")]
namespace DevInstance.LogScope.Logger
{
    internal class DefaultScopeManager : IScopeManager
    {
        public LogLevel BaseLevel { get; private set; }
        public ILogProvider Provider { get; }
        public IScopeFormatter Formater { get; }

        public DefaultScopeManager(LogLevel level, ILogProvider provider, IScopeFormatter formater)
        {
            if (provider == null || formater == null)
            {
                throw new ArgumentNullException();
            }

            BaseLevel = level;
            Provider = provider;
            Formater = formater;
        }

        public IScopeLog CreateLogger([CallerMemberName] string scope = null)
        {
            return new DefaultScopeLog(this, Formater, Provider, BaseLevel, scope, false);
        }

    }
}
