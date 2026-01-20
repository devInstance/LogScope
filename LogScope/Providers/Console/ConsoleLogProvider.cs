namespace DevInstance.LogScope.Providers.Console
{
    /// <summary>
    /// Log provider that writes output to the system console.
    /// </summary>
    /// <remarks>
    /// Error level messages are written to <see cref="System.Console.Error"/>,
    /// while all other messages are written to <see cref="System.Console.Out"/>.
    /// </remarks>
    public class ConsoleLogProvider : ILogProvider
    {
        /// <inheritdoc />
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
