using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using MLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace DevInstance.LogScope.Extensions.MicrosoftLogger
{
    internal class MicrosoftLogProvider : ILogProvider
    {
        private ILogger logger;

        public MicrosoftLogProvider(ILogger logger)
        {
            this.logger = logger;
        }

        public void WriteLine(LogLevel level, string line)
        {
            this.logger.Log(ConvertLevel(level), line);
        }

        public static MLogLevel ConvertLevel(LogLevel level)
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

    }
}
