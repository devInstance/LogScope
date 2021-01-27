using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope.Formaters
{
    public class DefaultFormater : ILogFormater
    {
        public bool ShowTimestamp { get; }
        public string ScopeSeparator { get; }

        public DefaultFormater(bool showTimestamp) : this(showTimestamp, ":")
        {

        }

        public DefaultFormater(bool showTimestamp, string scopeSeparator)
        {
            ShowTimestamp = showTimestamp;
            ScopeSeparator = scopeSeparator;
        }

        public string FormatLine(string scopeName, string message)
        {
            if (String.IsNullOrEmpty(scopeName))
            {
                return $"\t{message}";
            }
            return $"\t{scopeName}{ScopeSeparator}{message}";
        }

        public string FormatNestedScopes(string scopeName, string childScope)
        {
           return $"{scopeName}{ScopeSeparator}{childScope}";
        }

        public string ScopeStart(DateTime timeStart, string scopeName)
        {
            return $"--> begin of {scopeName}";
        }

        public string ScopeEnd(DateTime endTime, string scopeName, TimeSpan execTime)
        {
            return $"<-- end of {scopeName}, time:{execTime.TotalMilliseconds} msec";
        }
    }
}
