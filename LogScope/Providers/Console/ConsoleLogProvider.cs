using DevInstance.LogScope.Formaters;
using System.Runtime.CompilerServices;

namespace DevInstance.LogScope
{
    public class ConsoleLogProvider : ILogProvider
    {
        public LogLevel Level { get; private set; }
        public ILogFormater Formater { get; }

        public ConsoleLogProvider(LogLevel level) 
            : this(level, new DefaultFormater(false))
        {
            Level = level;
        }

        public ConsoleLogProvider(LogLevel level, ILogFormater formater)
        {
            Level = level;
            Formater = formater;
        }

        public ILog CreateLogger([CallerMemberName] string scope = null)
        {
            return new LogToConsole(this, Formater, Level, scope, false);
        }

    }
}
