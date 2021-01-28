
namespace DevInstance.LogScope
{
    public interface IScopeManager
    {
        ILogProvider Provider { get; }
        LogLevel Level { get; }

        IScopeLog CreateLogger(string scope);
    }
}
