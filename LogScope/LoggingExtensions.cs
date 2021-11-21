using System;
using System.Runtime.CompilerServices;

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

        public static IScopeLog CreateLogger(this IScopeManager provider, Object scope)
        {
            return provider.CreateLogger(scope.GetType().Name);
        }

        public static IScopeLog CreateLogger(this IScopeManager manager, Type scope, LogLevel levelOverride)
        {
            return manager.CreateLogger(scope.Name, levelOverride);
        }

        public static IScopeLog CreateLogger(this IScopeManager provider, Object scope, LogLevel levelOverride)
        {
            return provider.CreateLogger(scope.GetType().Name, levelOverride);
        }

        public static void T(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.TRACE, message);
            }
        }

        public static void D(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.DEBUG, message);
            }
        }

        public static void I(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.INFO, message);
            }
        }

        public static void W(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.WARNING, message);
            }
        }

        public static void E(this IScopeLog log, string message)
        {
            if (log != null)
            {
                log.Line(LogLevel.ERROR, $"Error!!!: {message}");
            }
        }

        public static void E(this IScopeLog log, Exception ex)
        {
            if (log != null)
            {
                log.Line(LogLevel.ERROR, $"Exception!!!: {ex.Message}");
                log.Line(LogLevel.ERROR, ex.StackTrace);
            }
        }

        public static void E(this IScopeLog log, string message, Exception ex)
        {
            if (log != null)
            {
                log.Line(LogLevel.ERROR, $"Exception!!!: {message}");
                log.Line(LogLevel.ERROR, ex.StackTrace);
            }
        }

        public static IScopeLog TraceScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log != null ? log.Scope(LogLevel.TRACE, scope) : null;
        }

        public static IScopeLog DebugScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log != null ? log.Scope(LogLevel.DEBUG, scope) : null;
        }

        public static IScopeLog InfoScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log != null ? log.Scope(LogLevel.INFO, scope) : null;
        }

        public static IScopeLog ErrorScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log != null ? log.Scope(LogLevel.ERROR, scope) : null;
        }
    }
}