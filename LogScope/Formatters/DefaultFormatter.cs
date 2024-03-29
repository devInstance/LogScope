﻿using System;
using System.Text;
using System.Threading;

namespace DevInstance.LogScope.Formatters
{
    public class DefaultFormattersOptions
    {
        public const string DefaultSeparator = ":";
        public const string DefaultIdSeparator = "|";
        public const string DefaultScopeStart = "--> ";
        public const string DefaultScopeEnd = "<-- ";

        public bool ShowTimestamp { get; set; }
        public bool ShowThreadNumber { get; set; }
        public bool ShowId { get; set; }
        public string Separator { get; set; }
        public string IdSeparator { get; set; }
        public string ScopeStart { get; set; }
        public string ScopeEnd { get; set; }

        public DefaultFormattersOptions()
        {
            Separator = Separator ?? DefaultSeparator;
            ScopeStart = ScopeStart ?? DefaultScopeStart;
            ScopeEnd = ScopeEnd ?? DefaultScopeEnd;
            IdSeparator = IdSeparator ?? DefaultIdSeparator;
        }
    }

    public class DefaultFormatter : IScopeFormatter
    {
        public DefaultFormattersOptions Options { get; }

        public DefaultFormatter(DefaultFormattersOptions options)
        {
            if(options != null)
            {
                Options = options;
            }
            else
            {
                Options = new DefaultFormattersOptions();
            }
        }

        public string FormatLine(string scopeName, string message)
        {
            StringBuilder result = new StringBuilder();
            AppendPrefix(result);
            result.Append('\t');
            if (!String.IsNullOrEmpty(scopeName))
            {
                result.Append(scopeName);
                result.Append(Options.Separator);
            }
            result.Append(message);
            return result.ToString();
        }

        private void AppendPrefix(StringBuilder result)
        {
            if (Options.ShowThreadNumber)
            {
                result.Append(Thread.CurrentThread.ManagedThreadId);
                result.Append(' ');
            }
            if (Options.ShowTimestamp)
            {
                result.AppendFormat("{0:yy-MM-dd HH:mm:ss}", DateTime.Now);
                result.Append(' ');
            }
        }

        public string FormatNestedScopes(string scopeName, string childScope)
        {
           return $"{scopeName}{Options.Separator}{childScope}";
        }

        public string ScopeStart(DateTime timeStart, string scopeName)
        {
            StringBuilder result = new StringBuilder();
            AppendPrefix(result);
            result.Append(Options.ScopeStart);
            result.Append(scopeName);

            return result.ToString();
        }

        public string ScopeEnd(DateTime endTime, string scopeName, TimeSpan execTime)
        {
            StringBuilder result = new StringBuilder();
            AppendPrefix(result);
            result.Append(Options.ScopeEnd);
            result.Append(scopeName);
            result.Append($", time:{ execTime.TotalMilliseconds} msec");
            return result.ToString();
        }

        public string ScopeStart(DateTime timeStart, IScopeLog scope)
        {
            StringBuilder result = new StringBuilder();
            AppendPrefix(result);
            result.Append(Options.ScopeStart);
            result.Append(scope.Name);
            AppendId(scope, result);

            return result.ToString();
        }

        private void AppendId(IScopeLog scope, StringBuilder result)
        {
            if (Options.ShowId)
            {
                result.Append(Options.IdSeparator);
                result.Append(scope.Id);
            }
        }

        public string ScopeEnd(DateTime endTime, IScopeLog scope, TimeSpan execTime)
        {
            StringBuilder result = new StringBuilder();
            AppendPrefix(result);
            result.Append(Options.ScopeEnd);
            result.Append(scope.Name);
            AppendId(scope, result);
            result.Append($", time:{ execTime.TotalMilliseconds} msec");
            return result.ToString();
        }

        public string FormatNestedScopes(string scopeName, IScopeLog childScope)
        {
            return $"{scopeName}{Options.Separator}{childScope.Name}";
        }

        public string FormatLine(IScopeLog scope, string message)
        {
            StringBuilder result = new StringBuilder();
            AppendPrefix(result);
            result.Append('\t');
            if (!String.IsNullOrEmpty(scope.Name))
            {
                result.Append(scope.Name);
                AppendId(scope, result);
                result.Append(Options.Separator);
            }
            result.Append(message);
            return result.ToString();
        }
    }
}
