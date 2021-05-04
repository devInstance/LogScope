namespace DevInstance.LogScope.Providers.Console
{
    public class ConsoleLogProvider : ILogProvider
    {
        public void WriteLine(LogLevel level, string line)
        {
            if(level == LogLevel.ERROR)
            {
                System.Console.Error.WriteLine(line);
            }
            else
            {
                System.Console.WriteLine(line);
            }
        }
    }
}
