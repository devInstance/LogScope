using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace DevInstance.LogScope.Templates
{
    internal static class MessageTemplate
    {
        public static string Render(string template, object[] args)
        {
            if (template == null)
                return string.Empty;

            if (args == null || args.Length == 0)
                return template;

            var sb = new StringBuilder(template.Length * 2);
            int argIndex = 0;
            int i = 0;

            while (i < template.Length)
            {
                char c = template[i];

                if (c == '{')
                {
                    if (i + 1 < template.Length && template[i + 1] == '{')
                    {
                        sb.Append('{');
                        i += 2;
                        continue;
                    }

                    int closeBrace = template.IndexOf('}', i + 1);
                    if (closeBrace == -1)
                    {
                        sb.Append(c);
                        i++;
                        continue;
                    }

                    string hole = template.Substring(i + 1, closeBrace - i - 1);

                    if (argIndex >= args.Length)
                    {
                        sb.Append('{');
                        sb.Append(hole);
                        sb.Append('}');
                    }
                    else
                    {
                        object arg = args[argIndex];
                        argIndex++;

                        bool destructure = false;
                        string format = null;

                        string name = hole;
                        int colonIndex = name.IndexOf(':');
                        if (colonIndex >= 0)
                        {
                            format = name.Substring(colonIndex + 1);
                            name = name.Substring(0, colonIndex);
                        }

                        if (name.Length > 0 && name[0] == '@')
                        {
                            destructure = true;
                        }

                        if (destructure)
                        {
                            sb.Append(Destructure(arg));
                        }
                        else if (format != null && arg is IFormattable formattable)
                        {
                            try
                            {
                                sb.Append(formattable.ToString(format, CultureInfo.InvariantCulture));
                            }
                            catch
                            {
                                sb.Append(arg?.ToString() ?? "null");
                            }
                        }
                        else
                        {
                            sb.Append(arg?.ToString() ?? "null");
                        }
                    }

                    i = closeBrace + 1;
                }
                else if (c == '}')
                {
                    if (i + 1 < template.Length && template[i + 1] == '}')
                    {
                        sb.Append('}');
                        i += 2;
                        continue;
                    }

                    sb.Append(c);
                    i++;
                }
                else
                {
                    sb.Append(c);
                    i++;
                }
            }

            return sb.ToString();
        }

        private static string Destructure(object obj)
        {
            if (obj == null)
                return "null";

            Type type = obj.GetType();

            if (type.IsPrimitive || obj is string || obj is decimal)
                return obj.ToString();

            if (obj is IEnumerable enumerable)
            {
                var sb = new StringBuilder();
                sb.Append('[');
                bool first = true;
                foreach (var item in enumerable)
                {
                    if (!first)
                        sb.Append(", ");
                    sb.Append(item?.ToString() ?? "null");
                    first = false;
                }
                sb.Append(']');
                return sb.ToString();
            }

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length > 0)
            {
                var sb = new StringBuilder();
                sb.Append("{ ");
                bool first = true;
                foreach (var prop in properties)
                {
                    if (prop.GetIndexParameters().Length > 0)
                        continue;

                    if (!first)
                        sb.Append(", ");

                    sb.Append(prop.Name);
                    sb.Append(": ");

                    try
                    {
                        object val = prop.GetValue(obj);
                        sb.Append(val?.ToString() ?? "null");
                    }
                    catch
                    {
                        sb.Append("<error>");
                    }

                    first = false;
                }
                sb.Append(" }");
                return sb.ToString();
            }

            return obj.ToString();
        }
    }
}
