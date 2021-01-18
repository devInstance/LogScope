
namespace DevInstance.LogScope
{
    public interface ILogProvider
    {
        LogLevel Level { get; }

        ILog CreateLogger(string scope);
    }
}
