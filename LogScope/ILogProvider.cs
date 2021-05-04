using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope
{
    /// <summary>
    /// ILogProvider defines the log provider interface. 
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Write log line.
        /// </summary>
        /// <param name="level">log level</param>
        /// <param name="line">reformatted string to log</param>
        void WriteLine(LogLevel level, string line);
    }
}
