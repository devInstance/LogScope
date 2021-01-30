using System;

namespace DevInstance.LogScope
{
    /// <summary>
    /// Log level
    /// </summary>
    public enum LogLevel
    {
        NOLOG = 0,
        ERROR = 2,
        INFO = 4,
        DEBUG = 8,
        DEBUG_EXTRA = 10
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IScopeLog : IDisposable
    {
        /// <summary>
        /// Name of the scope
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Log line
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Message</param>
        void Line(LogLevel level, string message);
        /// <summary>
        /// Create a nested scope
        /// </summary>
        /// <param name="level"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        IScopeLog Scope(LogLevel level, string scope);
    }
}
