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

        public DefaultScopeLog(IScopeManager manager, IScopeFormatter formater, ILogProvider provider, LogLevel scopeLevel, string scope, bool logConstructor)
        {
            if (manager == null || formater == null || provider == null)
            {
                throw new ArgumentNullException();
            }

            timeStart = DateTime.Now;
            ScopeLevel = scopeLevel;
            Name = scope;
            Manager = manager;
            Formatter = formater;
            Provider = provider;
            if (logConstructor && ScopeLevel <= manager.BaseLevel && !String.IsNullOrEmpty(Name))
            {
                Provider.WriteLine(ScopeLevel, formater.ScopeStart(timeStart, Name));
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
            return new DefaultScopeLog(Manager, Formatter, Provider, level, s, true);
        }

        public void Dispose()
        {
            if (ScopeLevel <= Manager.BaseLevel)
            {
                var endTime = DateTime.Now;
                var execTime = endTime - timeStart;
                Provider.WriteLine(ScopeLevel, Formatter.ScopeEnd(endTime, Name, execTime));
            }
        }

        public void Line(LogLevel l, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("There is no meaning in the empty message.");
            }

            if (l <= Manager.BaseLevel)
            {
                Provider.WriteLine(ScopeLevel, Formatter.FormatLine(Name, message));
            }
        }

        public void Line(string message)
        {
            Provider.WriteLine(ScopeLevel, Formatter.FormatLine(Name, message));
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
            return new DefaultScopeLog(Manager, Formatter, Provider, Manager.BaseLevel, s, true);
        }
    }
}
