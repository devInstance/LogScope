
namespace DevInstance.LogScope
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScopeManager
    {
        /// <summary>
        /// Provider <see cref="Provider"/>
        /// </summary>
        ILogProvider Provider { get; }
        /// <summary>
        /// Logging level <see cref="LogLevel"/>
        /// </summary>
        LogLevel Level { get; }
        /// <summary>
        /// Creates a main scope
        /// </summary>
        /// <param name="scope">scope's name</param>
        /// <returns>Reference to the scope <see cref="IScopeLog"/></returns>
        IScopeLog CreateLogger(string scope);
    }
}
