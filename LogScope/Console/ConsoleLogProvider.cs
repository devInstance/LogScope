using System.Runtime.CompilerServices;

namespace DevInstance.LogScope
{
    public class ConsoleLogProvider : ILogProvider
    {
        public LogLevel Level { get; private set; }

        public ConsoleLogProvider(LogLevel level)
        {
            Level = level;
        }

        public ILog CreateLogger([CallerMemberName] string scope = null)
        {
            return new LogToConsole(this, Level, scope, false);
        }

    }
}
