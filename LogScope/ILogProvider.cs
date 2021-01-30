using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        void WriteLine(string line);
    }
}
