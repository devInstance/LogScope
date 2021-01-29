using DevInstance.LogScope.Formaters;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LogScope.Tests")]
namespace DevInstance.LogScope.Logger
{
    internal class BaseScopeManager : IScopeManager
    {
        public LogLevel Level { get; private set; }
        public ILogProvider Provider { get; }
        public IScopeFormater Formater { get; }

        public BaseScopeManager(LogLevel level, ILogProvider provider)
            : this(level, provider, new DefaultFormater(false))
        {
        }

        public BaseScopeManager(LogLevel level, ILogProvider provider, IScopeFormater formater)
        {
            if (provider == null || formater == null)
            {
                throw new ArgumentNullException();
            }
            //Contract.Requires<ArgumentNullException>(provider != null);
            //Contract.Requires<ArgumentNullException>(formater != null);

            Level = level;
            Provider = provider;
            Formater = formater;
        }

        public IScopeLog CreateLogger([CallerMemberName] string scope = null)
        {
            return new BaseScopeLog(this, Formater, Level, scope, false);
        }

    }
}
