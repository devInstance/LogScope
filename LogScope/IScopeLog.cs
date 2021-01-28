using System;

namespace DevInstance.LogScope
{
    public enum LogLevel
    {
        NOLOG = 0,
        ERROR = 2,
        INFO = 4,
        DEBUG = 8,
        DEBUG_EXTRA = 10
    }

    public interface IScopeLog : IDisposable
    {
        void Line(LogLevel level, string message);
        IScopeLog Scope(LogLevel level, string scope);
    }
}
