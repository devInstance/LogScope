using Microsoft.Extensions.Logging;
using System;

using MLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace DevInstance.LogScope.Extensions.MicrosoftLogger
{
    internal class ScopeLog : IScopeLog
    {
        public LogLevel ScopeLevel { get; }

        private MLogLevel MLevel { get; }

        private DateTime timeStart;

        private ILogger logger;

        public string Name { get; }
        public IScopeFormatter Formatter { get; }
        public ILoggerFactory Factory { get; set; }

        public ScopeLog(ILoggerFactory factory, IScopeFormatter formatter, LogLevel scopeLevel, string scope, bool logConstructor)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException();
            }

            Factory = factory;

            this.logger = factory.CreateLogger(scope);

            timeStart = DateTime.Now;
            ScopeLevel = scopeLevel;
            MLevel = ConvertLevel(scopeLevel);
            Name = scope;
            Formatter = formatter;
            if (logConstructor && !String.IsNullOrEmpty(Name))
            {
                this.logger.Log(MLevel, formatter.ScopeStart(timeStart, Name));
            }

        }

        public void Dispose()
        {
            var endTime = DateTime.Now;
            var execTime = endTime - timeStart;
            this.logger.Log(MLevel, Formatter.ScopeEnd(endTime, Name, execTime));
        }

        private MLogLevel ConvertLevel(LogLevel level)
        {
            MLogLevel mLevel;
            switch (level)
            {
                case LogLevel.ERROR:
                    mLevel = MLogLevel.Error;
                    break;
                case LogLevel.WARNING:
                    mLevel = MLogLevel.Warning;
                    break;
                case LogLevel.INFO:
                    mLevel = MLogLevel.Information;
                    break;
                case LogLevel.DEBUG:
                    mLevel = MLogLevel.Debug;
                    break;
                case LogLevel.TRACE:
                    mLevel = MLogLevel.Trace;
                    break;
                case LogLevel.NOLOG:
                default:
                    mLevel = MLogLevel.None;
                    break;
            }

            return mLevel;
        }

        public void Line(LogLevel level, string message)
        {
            logger.Log(ConvertLevel(level), Formatter.FormatLine(Name, message));
        }
        public void Line(string message)
        {
            logger.Log(MLevel, message);
        }

        public IScopeLog Scope(LogLevel level, string scope)
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
            return new ScopeLog(Factory, Formatter, level, s, true);
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
            return new ScopeLog(Factory, Formatter, ScopeLevel, s, true);
        }
    }
}
