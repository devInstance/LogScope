using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("LogScope.Tests")]
namespace DevInstance.LogScope.Logger
{
    internal class BaseScopeLog : IScopeLog
    {
        private DateTime timeStart;
        public IScopeManager Manager { get; }
        public IScopeFormater Formater { get; }
        public LogLevel ScopeLevel { get; }
        public string ScopeName { get; }

        public BaseScopeLog(IScopeManager manager, IScopeFormater formater, LogLevel scopeLevel, string scope, bool logConstructor)
        {
            timeStart = DateTime.Now;
            ScopeLevel = scopeLevel;
            ScopeName = scope;
            Manager = manager;
            Formater = formater;
            if (logConstructor && ScopeLevel <= manager.Level && !String.IsNullOrEmpty(ScopeName))
            {
                manager.Provider.WriteLine(formater.ScopeStart(timeStart, ScopeName));
            }
        }

        public IScopeLog Scope(LogLevel level, string childScope)
        {
            var s = childScope;
            if (!String.IsNullOrEmpty(ScopeName))
            {
                s = Formater.FormatNestedScopes(ScopeName, childScope);
            }
            return new BaseScopeLog(Manager, Formater, level, s, true);
        }

        public void Dispose()
        {
            if (ScopeLevel <= Manager.Level)
            {
                var endTime = DateTime.Now;
                var execTime = endTime - timeStart;
                Manager.Provider.WriteLine(Formater.ScopeEnd(endTime, ScopeName, execTime));
            }
        }

        public void Line(LogLevel l, string message)
        {
            if (l <= Manager.Level)
            {
                Manager.Provider.WriteLine(Formater.FormatLine(ScopeName, message));
            }
        }
    }
}
