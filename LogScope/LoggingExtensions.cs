using System;
using System.Runtime.CompilerServices;

namespace DevInstance.LogScope
{
    public static class LoggingExtensions
    {
        public static ILog CreateLogger(this ILogProvider provider, Type scope)
        {
            return provider.CreateLogger(scope.Name);
        }

        public static ILog CreateLogger(this ILogProvider provider, Object scope)
        {
            return provider.CreateLogger(scope.GetType().Name);
        }

        public static void D(this ILog log, string message)
        {
            log.Line(LogLevel.DEBUG, message);
        }

        public static void I(this ILog log, string message)
        {
            log.Line(LogLevel.INFO, message);
        }

        public static void E(this ILog log, string message)
        {
            log.Line(LogLevel.ERROR, $"Error!!!: {message}");
        }
        public static void E(this ILog log, Exception ex)
        {
            log.Line(LogLevel.ERROR, $"Exception!!!: {ex.Message}");
            log.Line(LogLevel.ERROR, ex.StackTrace);
        }

        public static void E(this ILog log, string message, Exception ex)
        {
            log.Line(LogLevel.ERROR, $"Exception!!!: {message}");
            log.Line(LogLevel.ERROR, ex.StackTrace);
        }


        public static ILog DebugExScope(this ILog log, [CallerMemberName] string scope = "scope")
        {
            return log.CreateScope(LogLevel.DEBUG_EXTRA, scope);
        }
    }
}