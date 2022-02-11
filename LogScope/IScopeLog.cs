using System;

namespace DevInstance.LogScope
{
    /// <summary>
    /// Log level defines the severity of the log. Logging 
    /// level is optional and depends on the provider's implementation.
    /// <seealso cref="LoggingExtensions"/> for the helping methods to make
    /// it easier to use.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Used then log defined in logging provider configuration.
        /// </summary>
        UNDEFINED,
        /// <summary>
        /// No messages should be written in the log.
        /// </summary>
        NOLOG,
        /// <summary>
        /// Only errors and exception. <see cref="LoggingExtensions.E(IScopeLog, Exception)"/>
        /// </summary>
        ERROR,
        /// <summary>
        /// Warnings (including errors)
        /// </summary>
        WARNING,
        /// <summary>
        /// Information (preferred by default)
        /// </summary>
        INFO,
        /// <summary>
        /// Debug
        /// </summary>
        DEBUG,
        /// <summary>
        /// All messages
        /// </summary>
        TRACE
    }
    /// <summary>
    /// Logging scope. Scope can be method or a specific part of it. The implementation is 
    /// based on IDisposable where calling Dispose ends the scope. This the core interface where most of the "magic" happens.
    /// </summary>
    public interface IScopeLog : IDisposable
    {
        /// <summary>
        /// Name of the scope
        /// </summary>
        string Name { get; }
        /// <summary>
        /// A unique number of the instance scope
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Write one line
        /// </summary>
        /// <param name="level">Log level <see cref="LogLevel"/></param>
        /// <param name="message">Message</param>
        void Line(LogLevel level, string message);
        /// <summary>
        /// Write one line with default log level;
        /// </summary>
        /// <param name="message">Message</param>
        void Line(string message);
        /// <summary>
        /// Creates a nested scope
        /// </summary>
        /// <param name="level"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        IScopeLog Scope(LogLevel level, string scope);
        /// <summary>
        /// Creates a nested scope with default log level;
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        IScopeLog Scope(string scope);
        /// <summary>
        /// Returns current log level. Setting new level will override default value.
        /// </summary>
        LogLevel Level { get; }
    }
}
