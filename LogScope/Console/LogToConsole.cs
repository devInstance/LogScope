using System;

namespace DevInstance.LogScope 
{
    public class LogToConsole : ILog
    {
        private DateTime timeStart;
        public ILogProvider Provider { get; }
        public LogLevel ScopeLevel { get; }
        public string ScopeName { get; }

        public LogToConsole(ILogProvider provider, LogLevel scopeLevel, string scope, bool logConstructor)
        {
            timeStart = DateTime.Now;
            ScopeLevel = scopeLevel;
            ScopeName = scope;
            Provider = provider;
            if (logConstructor && ScopeLevel <= provider.Level && !String.IsNullOrEmpty(ScopeName))
            {
                Console.WriteLine($"--> begin of {ScopeName}");
            }
        }

        public ILog CreateScope(LogLevel level, string childScope)
        {
            var s = childScope;
            if (!String.IsNullOrEmpty(ScopeName))
            {
                s = $"{ScopeName}:{childScope}";
            }
            return new LogToConsole(Provider, level, s, true);
        }

        public void Dispose()
        {
            if (ScopeLevel <= Provider.Level)
            {
                var execTime = DateTime.Now - timeStart;
                Console.WriteLine($"<-- end of {ScopeName}, time:{execTime.TotalMilliseconds} msec");
            }
        }

        public void Line(LogLevel l, string message)
        {
            if (l <= Provider.Level)
            {
                if (String.IsNullOrEmpty(ScopeName))
                {
                    Console.WriteLine($"    {message}");
                }
                else
                {
                    Console.WriteLine($"    {ScopeName}: {message}");
                }
            }
        }
    }
}
