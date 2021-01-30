using System;
using System.Runtime.CompilerServices;

namespace DevInstance.LogScope
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggingExtensions
    {
        public static IScopeLog CreateLogger(this IScopeManager provider, Type scope)
        {
            return provider.CreateLogger(scope.Name);
        }

        public static IScopeLog CreateLogger(this IScopeManager provider, Object scope)
        {
            return provider.CreateLogger(scope.GetType().Name);
        }

        public static void DE(this IScopeLog log, string message)
        {
            log.Line(LogLevel.DEBUG_EXTRA, message);
        }

        public static void D(this IScopeLog log, string message)
        {
            log.Line(LogLevel.DEBUG, message);
        }

        public static void I(this IScopeLog log, string message)
        {
            log.Line(LogLevel.INFO, message);
        }

        public static void E(this IScopeLog log, string message)
        {
            log.Line(LogLevel.ERROR, $"Error!!!: {message}");
        }
        public static void E(this IScopeLog log, Exception ex)
        {
            log.Line(LogLevel.ERROR, $"Exception!!!: {ex.Message}");
            log.Line(LogLevel.ERROR, ex.StackTrace);
        }

        public static void E(this IScopeLog log, string message, Exception ex)
        {
            log.Line(LogLevel.ERROR, $"Exception!!!: {message}");
            log.Line(LogLevel.ERROR, ex.StackTrace);
        }

        public static IScopeLog DebugExScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log.Scope(LogLevel.DEBUG_EXTRA, scope);
        }

        public static IScopeLog DebugScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log.Scope(LogLevel.DEBUG, scope);
        }

        public static IScopeLog InfoScope(this IScopeLog log, [CallerMemberName] string scope = "scope")
        {
            return log.Scope(LogLevel.DEBUG, scope);
        }
    }
}