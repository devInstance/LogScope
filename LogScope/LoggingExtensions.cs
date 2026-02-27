using System;
using System.Runtime.CompilerServices;
using DevInstance.LogScope.Templates;

namespace DevInstance.LogScope
{
    /// <summary>
    /// The goals of extension functions is to maintain simplicity and keep the code clean.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Create a scope
        /// </summary>
        /// <param name="manager">scope manager</param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static IScopeLog CreateLogger(this IScopeManager manager, Type scope)
        {
            return manager.CreateLogger(scope.Name);
        }

        /// <summary>
        /// Creates a logger scope using the object's type name as the scope name.
        /// </summary>
        /// <param name="provider">The scope manager.</param>
        /// <param name="scope">An object whose type name will be used as the scope name.</param>
        /// <returns>A new <see cref="IScopeLog"/> instance.</returns>
        public static IScopeLog CreateLogger(this IScopeManager provider, Object scope)
        {
            return provider.CreateLogger(scope.GetType().Name);
        }

        /// <summary>
        /// Creates a logger scope using the type name with a custom log level.
        /// </summary>
        /// <param name="manager">The scope manager.</param>
        /// <param name="scope">A Type whose name will be used as the scope name.</param>
        /// <param name="levelOverride">The log level override for this scope.</param>
        /// <returns>A new <see cref="IScopeLog"/> instance.</returns>
        public static IScopeLog CreateLogger(this IScopeManager manager, Type scope, LogLevel levelOverride)
        {
            return manager.CreateLogger(scope.Name, levelOverride);
        }

        /// <summary>
        /// Creates a logger scope using the object's type name with a custom log level.
        /// </summary>
        /// <param name="provider">The scope manager.</param>
        /// <param name="scope">An object whose type name will be used as the scope name.</param>
        /// <param name="levelOverride">The log level override for this scope.</param>
        /// <returns>A new <see cref="IScopeLog"/> instance.</returns>
        public static IScopeLog CreateLogger(this IScopeManager provider, Object scope, LogLevel levelOverride)
        {
            return provider.CreateLogger(scope.GetType().Name, levelOverride);
        }

        /// <summary>
        /// Writes a TRACE level message. Use for highly detailed diagnostic information.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="message">The message to write.</param>
        public static void T(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.TRACE, message);
            }
        }

        /// <summary>
        /// Writes a DEBUG level message. Use for debugging during development.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="message">The message to write.</param>
        public static void D(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.DEBUG, message);
            }
        }

        /// <summary>
        /// Writes an INFO level message. Use for general informational messages.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="message">The message to write.</param>
        public static void I(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.INFO, message);
            }
        }

        /// <summary>
        /// Writes a WARNING level message. Use to indicate potential issues.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="message">The message to write.</param>
        public static void W(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.WARNING, message);
            }
        }

        /// <summary>
        /// Writes an ERROR level message.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="message">The error message to write.</param>
        public static void E(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.ERROR, $"Error!!!: {message}");
            }
        }

        /// <summary>
        /// Writes an ERROR level message with exception details including stack trace.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="ex">The exception to log.</param>
        public static void E(this IScopeLog log, Exception ex)
        {
            if (log != null)
            {
                log.Line(LogLevel.ERROR, $"Exception!!!: {ex.Message}");
                log.Line(LogLevel.ERROR, ex.StackTrace);
            }
        }

        /// <summary>
        /// Writes an ERROR level message with a custom message and exception stack trace.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="message">The custom error message.</param>
        /// <param name="ex">The exception to log.</param>
        public static void E(this IScopeLog log, string message, Exception ex)
        {
            if (log != null)
            {
                log.Line(LogLevel.ERROR, $"Exception!!!: {message}");
                log.Line(LogLevel.ERROR, ex.StackTrace);
            }
        }

        /// <summary>
        /// Writes a TRACE level message using a message template with structured arguments.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="messageTemplate">The message template with placeholders (e.g., "Processed {Count} items").</param>
        /// <param name="args">The arguments to fill template placeholders.</param>
        public static void T(this IScopeLog log, string messageTemplate, params object[] args)
        {
            if (log != null)
            {
                log.Line(LogLevel.TRACE, MessageTemplate.Render(messageTemplate, args));
            }
        }

        /// <summary>
        /// Writes a DEBUG level message using a message template with structured arguments.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="messageTemplate">The message template with placeholders.</param>
        /// <param name="args">The arguments to fill template placeholders.</param>
        public static void D(this IScopeLog log, string messageTemplate, params object[] args)
        {
            if (log != null)
            {
                log.Line(LogLevel.DEBUG, MessageTemplate.Render(messageTemplate, args));
            }
        }

        /// <summary>
        /// Writes an INFO level message using a message template with structured arguments.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="messageTemplate">The message template with placeholders.</param>
        /// <param name="args">The arguments to fill template placeholders.</param>
        public static void I(this IScopeLog log, string messageTemplate, params object[] args)
        {
            if (log != null)
            {
                log.Line(LogLevel.INFO, MessageTemplate.Render(messageTemplate, args));
            }
        }

        /// <summary>
        /// Writes a WARNING level message using a message template with structured arguments.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="messageTemplate">The message template with placeholders.</param>
        /// <param name="args">The arguments to fill template placeholders.</param>
        public static void W(this IScopeLog log, string messageTemplate, params object[] args)
        {
            if (log != null)
            {
                log.Line(LogLevel.WARNING, MessageTemplate.Render(messageTemplate, args));
            }
        }

        /// <summary>
        /// Writes an ERROR level message using a message template with structured arguments.
        /// </summary>
        /// <param name="log">The scope log instance.</param>
        /// <param name="messageTemplate">The message template with placeholders.</param>
        /// <param name="args">The arguments to fill template placeholders.</param>
        public static void E(this IScopeLog log, string messageTemplate, params object[] args)
        {
            if (log != null)
            {
                log.Line(LogLevel.ERROR, $"Error!!!: {MessageTemplate.Render(messageTemplate, args)}");
            }
        }

        /// <summary>
        /// Creates a nested TRACE level scope. Automatically uses the calling method name if no scope name is provided.
        /// </summary>
        /// <param name="log">The parent scope log.</param>
        /// <param name="scope">The scope name (defaults to calling method name).</param>
        /// <returns>A new nested <see cref="IScopeLog"/> at TRACE level, or null if log is null.</returns>
        public static IScopeLog TraceScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log != null ? log.Scope(LogLevel.TRACE, scope) : null;
        }

        /// <summary>
        /// Creates a nested DEBUG level scope. Automatically uses the calling method name if no scope name is provided.
        /// </summary>
        /// <param name="log">The parent scope log.</param>
        /// <param name="scope">The scope name (defaults to calling method name).</param>
        /// <returns>A new nested <see cref="IScopeLog"/> at DEBUG level, or null if log is null.</returns>
        public static IScopeLog DebugScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log != null ? log.Scope(LogLevel.DEBUG, scope) : null;
        }

        /// <summary>
        /// Creates a nested INFO level scope. Automatically uses the calling method name if no scope name is provided.
        /// </summary>
        /// <param name="log">The parent scope log.</param>
        /// <param name="scope">The scope name (defaults to calling method name).</param>
        /// <returns>A new nested <see cref="IScopeLog"/> at INFO level, or null if log is null.</returns>
        public static IScopeLog InfoScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log != null ? log.Scope(LogLevel.INFO, scope) : null;
        }

        /// <summary>
        /// Creates a nested ERROR level scope. Automatically uses the calling method name if no scope name is provided.
        /// </summary>
        /// <param name="log">The parent scope log.</param>
        /// <param name="scope">The scope name (defaults to calling method name).</param>
        /// <returns>A new nested <see cref="IScopeLog"/> at ERROR level, or null if log is null.</returns>
        public static IScopeLog ErrorScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log != null ? log.Scope(LogLevel.ERROR, scope) : null;
        }
    }
}