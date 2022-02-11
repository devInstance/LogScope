using DevInstance.LogScope.Utils;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LogScope.Tests")]
[assembly: InternalsVisibleTo("DevInstance.LogScope.Extensions.MicrosoftLogger")]
namespace DevInstance.LogScope.Logger
{
    internal class DefaultScopeLog : IScopeLog
    {
        private DateTime timeStart;
        public ILogProvider Provider { get; }
        public IScopeManager Manager { get; }
        public IScopeFormatter Formatter { get; }
        public LogLevel ScopeLevel { get; }
        public string Name { get; }
        public string Id { get;  }
        public LogLevel Level { get => BaseLevel; }

        private LogLevel BaseLevel;

        public DefaultScopeLog(IScopeManager manager, 
                                LogLevel baseLevel, 
                                IScopeFormatter formater, 
                                ILogProvider provider, 
                                LogLevel scopeLevel, 
                                string scope, 
                                bool logConstructor)
        {
            if (manager == null || formater == null || provider == null)
            {
                throw new ArgumentNullException();
            }

            timeStart = DateTime.Now;
            ScopeLevel = scopeLevel;
            BaseLevel = baseLevel;
            Name = scope;
            Id = Guid.NewGuid().ToString();// IdGenerator.FromGuid(Guid.NewGuid());
            Manager = manager;
            Formatter = formater;
            Provider = provider;
            if (logConstructor && ScopeLevel <= BaseLevel && !String.IsNullOrEmpty(Name))
            {
                Provider.WriteLine(ScopeLevel, formater.ScopeStart(timeStart, this));
            }
        }

        public IScopeLog Scope(LogLevel level, string childScope)
        {
            if (String.IsNullOrEmpty(childScope))
            {
                throw new ArgumentException("There is no reason of having scope without name.");
            }

            var s = childScope;
            if (!String.IsNullOrEmpty(Name))
            {
                s = Formatter.FormatNestedScopes(Name, childScope);
            }
            return new DefaultScopeLog(Manager, BaseLevel, Formatter, Provider, level, s, true);
        }

        public void Dispose()
        {
            if (ScopeLevel <= BaseLevel)
            {
                var endTime = DateTime.Now;
                var execTime = endTime - timeStart;
                Provider.WriteLine(ScopeLevel, Formatter.ScopeEnd(endTime, this, execTime));
            }
        }

        public void Line(LogLevel l, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("There is no meaning in the empty message.");
            }

            if (l <= BaseLevel)
            {
                Provider.WriteLine(ScopeLevel, Formatter.FormatLine(this, message));
            }
        }

        public void Line(string message)
        {
            Provider.WriteLine(ScopeLevel, Formatter.FormatLine(this, message));
        }

        public IScopeLog Scope(string scope)
        {
            if (String.IsNullOrEmpty(scope))
            {
                throw new ArgumentException("There is no reason of having scope without name.");
            }

            var s = scope;
            if (!String.IsNullOrEmpty(Name))
            {
                s = Formatter.FormatNestedScopes(Name, scope);
            }
            return new DefaultScopeLog(Manager, BaseLevel, Formatter, Provider, Manager.BaseLevel, s, true);
        }
    }
}
