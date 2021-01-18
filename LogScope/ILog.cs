using System;

namespace DevInstance.LogScope
{
    public enum LogLevel
    {
        ERROR = 0,
        INFO = 4,
        DEBUG = 8,
        DEBUG_EXTRA = 10
    }

    public interface ILog : IDisposable
    {
        void Line(LogLevel level, string message);
        ILog CreateScope(LogLevel level, string scope);
    }
}
