using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope
{
    /// <summary>
    /// IScopeFormatter defines the formatter interface. Every scope has start
    ///  and end, can contain nested scope and lines between start an end. In
    ///  addition to it, every scope can log the time of execution. Formatter
    ///  interfaces describes the contract for developing customize formatter.
    /// </summary>
    public interface IScopeFormatter
    {
        /// <summary>
        /// Called when scope starts
        /// </summary>
        /// <param name="timeStart">time of the scope's creation</param>
        /// <param name="scopeName">scope name</param>
        /// <returns>formated string</returns>
        [Obsolete]
        string ScopeStart(DateTime timeStart, string scopeName);
        /// <summary>
        /// Called when scope ends
        /// </summary>
        /// <param name="endTime">time of the scope's end</param>
        /// <param name="scopeName">scope name</param>
        /// <param name="execTime">total time between start and end</param>
        /// <returns></returns>
        [Obsolete]
        string ScopeEnd(DateTime endTime, string scopeName, TimeSpan execTime);
        /// <summary>
        /// Constructs full scope name, a combination parent and child scope
        /// </summary>
        /// <param name="scopeName">parent scope name</param>
        /// <param name="childScope">child scope name</param>
        /// <returns>return full name. Example: 'parent:child'</returns>
        string FormatNestedScopes(string scopeName, string childScope);
        /// <summary>
        /// Returns formated message
        /// </summary>
        /// <param name="scopeName">scope name</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        [Obsolete]
        string FormatLine(string scopeName, string message);

        /// <summary>
        /// Called when scope starts
        /// </summary>
        /// <param name="timeStart">time of the scope's creation</param>
        /// <param name="scope">the scope that is starting</param>
        /// <returns>formated string</returns>
        string ScopeStart(DateTime timeStart, IScopeLog scope);
        /// <summary>
        /// Called when scope ends
        /// </summary>
        /// <param name="endTime">time of the scope's end</param>
        /// <param name="scope">the scope that is ending</param>
        /// <param name="execTime">total time between start and end</param>
        /// <returns>formatted string</returns>
        string ScopeEnd(DateTime endTime, IScopeLog scope, TimeSpan execTime);
        /// <summary>
        /// Constructs full scope name, a combination parent and child scope
        /// </summary>
        /// <param name="scopeName">parent scope name</param>
        /// <param name="childScope">child scope</param>
        /// <returns>return full name. Example: 'parent:child'</returns>
        string FormatNestedScopes(string scopeName, IScopeLog childScope);
        /// <summary>
        /// Returns formated message
        /// </summary>
        /// <param name="scope">the scope</param>
        /// <param name="message">message</param>
        /// <returns>formatted string</returns>
        string FormatLine(IScopeLog scope, string message);
    }
}
