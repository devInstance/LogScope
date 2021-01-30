using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScopeFormater
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        string ScopeStart(DateTime timeStart, string scopeName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endTime"></param>
        /// <param name="scopeName"></param>
        /// <param name="execTime"></param>
        /// <returns></returns>
        string ScopeEnd(DateTime endTime, string scopeName, TimeSpan execTime);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="childScope"></param>
        /// <returns></returns>
        string FormatNestedScopes(string scopeName, string childScope);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scopeName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        string FormatLine(string scopeName, string message);
    }
}
