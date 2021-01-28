using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope
{
    public interface IScopeFormater
    {
        string ScopeStart(DateTime timeStart, string scopeName);
        string ScopeEnd(DateTime endTime, string scopeName, TimeSpan execTime);
        string FormatNestedScopes(string scopeName, string childScope);
        string FormatLine(string scopeName, string message);
    }
}
