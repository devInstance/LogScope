using System;
using System.Text;
using System.Threading;

namespace DevInstance.LogScope.Formatters
{
    /// <summary>
    /// Configuration options for the <see cref="DefaultFormatter"/>.
    /// </summary>
    /// <remarks>
    /// Use these options to customize the output format of log messages,
    /// including timestamps, thread numbers, scope identifiers, and separators.
    /// </remarks>
    public class DefaultFormattersOptions
    {
        /// <summary>
        /// The default separator between scope names (":")
        /// </summary>
        public const string DefaultSeparator = ":";
        /// <summary>
        /// The default separator for scope IDs ("|")
        /// </summary>
        public const string DefaultIdSeparator = "|";
        /// <summary>
        /// The default prefix for scope start messages ("--&gt; ")
        /// </summary>
        public const string DefaultScopeStart = "--> ";
        /// <summary>
        /// The default prefix for scope end messages ("&lt;-- ")
        /// </summary>
        public const string DefaultScopeEnd = "<-- ";

        /// <summary>
        /// Gets or sets whether to include timestamps in log output.
        /// </summary>
        public bool ShowTimestamp { get; set; }
        /// <summary>
        /// Gets or sets whether to include the thread ID in log output.
        /// </summary>
        public bool ShowThreadNumber { get; set; }
        /// <summary>
        /// Gets or sets whether to include the unique scope ID in log output.
        /// </summary>
        public bool ShowId { get; set; }
        /// <summary>
        /// Gets or sets the separator string between scope names. Default is ":".
        /// </summary>
        public string Separator { get; set; }
        /// <summary>
        /// Gets or sets the separator string before the scope ID. Default is "|".
        /// </summary>
        public string IdSeparator { get; set; }
        /// <summary>
        /// Gets or sets the prefix string for scope start messages. Default is "--&gt; ".
        /// </summary>
        public string ScopeStart { get; set; }
        /// <summary>
        /// Gets or sets the prefix string for scope end messages. Default is "&lt;-- ".
        /// </summary>
        public string ScopeEnd { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultFormattersOptions"/> with default values.
        /// </summary>
        public DefaultFormattersOptions()
        {
            Separator = Separator ?? DefaultSeparator;
            ScopeStart = ScopeStart ?? DefaultScopeStart;
            ScopeEnd = ScopeEnd ?? DefaultScopeEnd;
            IdSeparator = IdSeparator ?? DefaultIdSeparator;
        }
    }

    /// <summary>
    /// Default implementation of <see cref="IScopeFormatter"/> that formats log output
    /// with configurable timestamps, thread numbers, and scope identifiers.
    /// </summary>
    public class DefaultFormatter : IScopeFormatter
    {
        /// <summary>
        /// Gets the formatter options.
        /// </summary>
        public DefaultFormattersOptions Options { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultFormatter"/> with the specified options.
        /// </summary>
        /// <param name="options">The formatter options, or null to use defaults.</param>
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public string FormatNestedScopes(string scopeName, string childScope)
        {
           return $"{scopeName}{Options.Separator}{childScope}";
        }

        /// <inheritdoc />
        public string ScopeStart(DateTime timeStart, string scopeName)
        {
            StringBuilder result = new StringBuilder();
            AppendPrefix(result);
            result.Append(Options.ScopeStart);
            result.Append(scopeName);

            return result.ToString();
        }

        /// <inheritdoc />
        public string ScopeEnd(DateTime endTime, string scopeName, TimeSpan execTime)
        {
            StringBuilder result = new StringBuilder();
            AppendPrefix(result);
            result.Append(Options.ScopeEnd);
            result.Append(scopeName);
            result.Append($", time:{ execTime.TotalMilliseconds} msec");
            return result.ToString();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public string FormatNestedScopes(string scopeName, IScopeLog childScope)
        {
            return $"{scopeName}{Options.Separator}{childScope.Name}";
        }

        /// <inheritdoc />
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
