using System;

namespace DevInstance.LogScope 
{
    internal class LogToConsole : ILog
    {
        private DateTime timeStart;
        public ILogProvider Provider { get; }
        public ILogFormater Formater { get; }
        public LogLevel ScopeLevel { get; }
        public string ScopeName { get; }

        public LogToConsole(ILogProvider provider, ILogFormater formater, LogLevel scopeLevel, string scope, bool logConstructor)
        {
            timeStart = DateTime.Now;
            ScopeLevel = scopeLevel;
            ScopeName = scope;
            Provider = provider;
            Formater = formater;
            if (logConstructor && ScopeLevel <= provider.Level && !String.IsNullOrEmpty(ScopeName))
            {
                Console.WriteLine(formater.ScopeStart(timeStart, ScopeName));
            }
        }

        public ILog Scope(LogLevel level, string childScope)
        {
            var s = childScope;
            if (!String.IsNullOrEmpty(ScopeName))
            {
                s = Formater.FormatNestedScopes(ScopeName, childScope);
            }
            return new LogToConsole(Provider, Formater, level, s, true);
        }

        public void Dispose()
        {
            if (ScopeLevel <= Provider.Level)
            {
                var endTime = DateTime.Now;
                var execTime = endTime - timeStart;
                Console.WriteLine(Formater.ScopeEnd(endTime, ScopeName, execTime));
            }
        }

        public void Line(LogLevel l, string message)
        {
            if (l <= Provider.Level)
            {
                Console.WriteLine(Formater.FormatLine(ScopeName, message));
            }
        }
    }
}
