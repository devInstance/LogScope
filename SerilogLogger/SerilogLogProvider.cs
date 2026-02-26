using Serilog;
using Serilog.Events;

using SLogLevel = Serilog.Events.LogEventLevel;

namespace DevInstance.LogScope.Extensions.SerilogLogger
{
    internal class SerilogLogProvider : ILogProvider
    {
        private ILogger logger;

        public SerilogLogProvider(ILogger logger)
        {
            this.logger = logger;
        }

        public void WriteLine(LogLevel level, string line)
        {
            this.logger.Write(ConvertLevel(level), line);
        }

        public static SLogLevel ConvertLevel(LogLevel level)
        {
            SLogLevel sLevel;
            switch (level)
            {
                case LogLevel.ERROR:
                    sLevel = SLogLevel.Error;
                    break;
                case LogLevel.WARNING:
                    sLevel = SLogLevel.Warning;
                    break;
                case LogLevel.INFO:
                    sLevel = SLogLevel.Information;
                    break;
                case LogLevel.DEBUG:
                    sLevel = SLogLevel.Debug;
                    break;
                case LogLevel.TRACE:
                    sLevel = SLogLevel.Verbose;
                    break;
                case LogLevel.NOLOG:
                default:
                    sLevel = SLogLevel.Verbose;
                    break;
            }

            return sLevel;
        }

    }
}
