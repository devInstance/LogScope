using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LogScope.Tests")]
namespace DevInstance.LogScope.Logger
{
    internal class BaseScopeLog : IScopeLog
    {
        private DateTime timeStart;
        public IScopeManager Manager { get; }
        public IScopeFormater Formater { get; }
        public LogLevel ScopeLevel { get; }
        public string Name { get; }

        public BaseScopeLog(IScopeManager manager, IScopeFormater formater, LogLevel scopeLevel, string scope, bool logConstructor)
        {
            if (manager == null || formater == null)
            {
                throw new ArgumentNullException();
            }

            //Contract.Requires<ArgumentNullException>(manager != null);
            //Contract.Requires<ArgumentNullException>(formater != null);

            timeStart = DateTime.Now;
            ScopeLevel = scopeLevel;
            Name = scope;
            Manager = manager;
            Formater = formater;
            if (logConstructor && ScopeLevel <= manager.Level && !String.IsNullOrEmpty(Name))
            {
                manager.Provider.WriteLine(formater.ScopeStart(timeStart, Name));
            }
        }

        public IScopeLog Scope(LogLevel level, string childScope)
        {
            //Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(childScope), "There is no reason of having scope without name.");
            if (String.IsNullOrEmpty(childScope))
            {
                throw new ArgumentException("There is no reason of having scope without name.");
            }

            var s = childScope;
            if (!String.IsNullOrEmpty(Name))
            {
                s = Formater.FormatNestedScopes(Name, childScope);
            }
            return new BaseScopeLog(Manager, Formater, level, s, true);
        }

        public void Dispose()
        {
            if (ScopeLevel <= Manager.Level)
            {
                var endTime = DateTime.Now;
                var execTime = endTime - timeStart;
                Manager.Provider.WriteLine(Formater.ScopeEnd(endTime, Name, execTime));
            }
        }

        public void Line(LogLevel l, string message)
        {
            //Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(message), "There is no meaning in the empty message.");
            if (String.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("There is no meaning in the empty message.");
            }

            if (l <= Manager.Level)
            {
                Manager.Provider.WriteLine(Formater.FormatLine(Name, message));
            }
        }
    }
}
