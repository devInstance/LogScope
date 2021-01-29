using System;
using System.Collections.Generic;
using System.Text;

namespace DevInstance.LogScope.Formaters
{
    public class DefaultFormater : IScopeFormater
    {
        public const string DefaultSeparator = ":";
        public const string DefaultScopeStart = "--> begin of ";
        public const string DefaultScopeEnd = "<-- end of ";

        public bool ShowTimestamp { get; }
        public string Separator { get; }
        public string ScopeMessageStart { get; }
        public string ScopeMessageEnd { get; }

        public DefaultFormater(bool showTimestamp) : this(showTimestamp, DefaultSeparator, DefaultScopeStart, DefaultScopeEnd)
        {

        }

        public DefaultFormater(bool showTimestamp, string separator, string scopeStart, string scopeEnd)
        {
            ShowTimestamp = showTimestamp;
            Separator = separator;
            ScopeMessageStart = scopeStart;
            ScopeMessageEnd = scopeEnd;
        }

        public string FormatLine(string scopeName, string message)
        {
            var result = "";
            if(ShowTimestamp)
            {
                result += String.Format("yy-MM-dd HH:mm:ss", DateTime.Now);
            }
            if (String.IsNullOrEmpty(scopeName))
            {
                return $"{result}\t{message}";
            }
            return $"{result}\t{scopeName}{Separator}{message}";
        }

        public string FormatNestedScopes(string scopeName, string childScope)
        {
           return $"{scopeName}{Separator}{childScope}";
        }

        public string ScopeStart(DateTime timeStart, string scopeName)
        {
            return $"{ScopeMessageStart}{scopeName}";
        }

        public string ScopeEnd(DateTime endTime, string scopeName, TimeSpan execTime)
        {
            return $"{ScopeMessageEnd}{scopeName}, time:{execTime.TotalMilliseconds} msec";
        }
    }
}
