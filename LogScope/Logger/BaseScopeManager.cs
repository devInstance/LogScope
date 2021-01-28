using DevInstance.LogScope.Formaters;
using System.Runtime.CompilerServices;

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
