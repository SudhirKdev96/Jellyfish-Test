using System;
using System.Text;

namespace WebUI.Data
{
    /// <summary>
    /// Defines extension methods for all StringBuilders
    /// </summary>
    public static class StringBuilderExtension
    {
        /// <summary>
        /// Append text to this builder only if it's not null, empty, or whitespace only
        /// </summary>
        /// <param name="value">text to append, or format and append</param>
        /// <param name="appendLine">should the text be appended with a line terminator?</param>
        /// <param name="formatString">(Optional) format the given value using this before appending</param>
        public static void AppendNonEmpty(this StringBuilder sb, string value, bool appendLine, string formatString = null)
        {
            if (sb == null || !value.HasValue()) return;

            string text = (formatString.HasValue()) ? String.Format(formatString, value) : value;

            if (appendLine)
            {
                sb.AppendLine(text);
            }
            else
            {
                sb.Append(text);
            }
        }
    }
}
