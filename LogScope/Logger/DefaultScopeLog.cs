using DevInstance.LogScope.Utils;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LogScope.Tests")]
[assembly: InternalsVisibleTo("DevInstance.LogScope.Extensions.MicrosoftLogger")]
namespace DevInstance.LogScope.Logger
{
    internal class DefaultScopeLog : IScopeLog
    {
        private LContext context;
        private DateTime timeStart;
        public ILogProvider Provider { get => context.Provider; }
        public IScopeManager Manager { get => context.Manager; }
        public IScopeFormatter Formatter { get => context.Formatter; }
        public LogLevel ScopeLevel { get; }

        private string name = null;
        public string Name
        {
            get
            {
                if (name == null)
                {
                    if (parentScope != null)
                    {
                        name = Formatter.FormatNestedScopes(parentScope, childScope);
                    }
                    else
                    {
                        name = childScope;
                    }
                }
                return name;
            }
        }

        private string parentScope;
        private string childScope;

        private string scopeId = null;
        public string Id
        {
            get
            {
                if (scopeId == null)
                {
                    scopeId = Guid.NewGuid().ToString();
                }
                return scopeId;
            }
        }
        public LogLevel Level { get => BaseLevel; }

        private LogLevel BaseLevel;

        public DefaultScopeLog(LContext ctx,
                                LogLevel baseLevel,
                                LogLevel scopeLevel,
                                string prnttScope,
                                string chldScope,
                                bool logConstructor)
        {
            ScopeLevel = scopeLevel;
            BaseLevel = baseLevel;
            childScope = chldScope;
            parentScope = prnttScope;
            context = ctx;

            if (ScopeLevel <= BaseLevel)
            {
                timeStart = DateTime.Now;
                if (logConstructor && Name != null)
                {
                    Provider.WriteLine(ScopeLevel, context.Formatter.ScopeStart(timeStart, this));
                }
            }
        }

        public IScopeLog Scope(LogLevel level, string childScope)
        {
#if DEBUG
            if (String.IsNullOrEmpty(childScope))
            {
                throw new ArgumentException("There is no reason of having scope without name.");
            }
#endif
            return new DefaultScopeLog(context, BaseLevel, level, Name, childScope, true);
        }

        public IScopeLog Scope(string scope)
        {
            return Scope(Manager.BaseLevel, scope);
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
            if (l <= BaseLevel)
            {
                Provider.WriteLine(ScopeLevel, Formatter.FormatLine(this, message));
            }
        }

        public void Line(string message)
        {
            Provider.WriteLine(ScopeLevel, Formatter.FormatLine(this, message));
        }
    }
}
