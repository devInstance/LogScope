namespace DevInstance.LogScope.Providers.Console
{
    public class ConsoleLogProvider : ILogProvider
    {
        public void WriteLine(string line)
        {
            System.Console.WriteLine(line);
        }
    }
}
