
namespace DevInstance.LogScope
{
    /// <summary>
    /// IScopeManager defines the log manager. Use <see cref="DefaultScopeLogFactory" />
    /// or <see cref="ServiceExtensions" />.
    /// </summary>
    public interface IScopeManager
    {
        /// <summary>
        /// Base logging level that defines the minimum logging level for the application. <see cref="LogLevel"/>
        /// </summary>
        LogLevel BaseLevel { get; }
        /// <summary>
        /// Creates a main scope
        /// </summary>
        /// <param name="scope">scope's name</param>
        /// <returns>Reference to the scope <see cref="IScopeLog"/></returns>
        IScopeLog CreateLogger(string scope);
        /// <summary>
        /// Creates a main scope with custom logging level. This method should be used for
        /// debug purposes only.
        /// </summary>
        /// <param name="scope">scope's name</param>
        /// <param name="levelOverride">level override</param>
        /// <returns></returns>
        IScopeLog CreateLogger(string scope, LogLevel levelOverride);
    }
}
