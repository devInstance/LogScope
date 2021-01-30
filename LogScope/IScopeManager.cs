
namespace DevInstance.LogScope
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScopeManager
    {
        /// <summary>
        /// 
        /// </summary>
        ILogProvider Provider { get; }
        /// <summary>
        /// 
        /// </summary>
        LogLevel Level { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        IScopeLog CreateLogger(string scope);
    }
}
