using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WebUI.Data.Models;

namespace WebUI.Data
{
    public static class StringExtension
    {
        /// <summary>
        /// A convenience to avoid tediously long lines of code for a common check 
        /// that the given string is not null, empty, or comprised only of whitespace.
        /// Equivalent to !String.IsNullOrWhiteSpace(s)
        /// </summary>
        public static bool HasValue(this string s)
        {
            return !String.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// A convenience to convert a string to Title Case using the current culture
        /// </summary>
        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }

        /// <summary>
        /// Capitalize only the first character of the given string (if it has any characters in it)
        /// </summary>
        public static string Capitalize(this string s)
        {
            if (!s.HasValue()) { return s; }

            return String.Concat(s[0].ToString().ToUpper(), s.AsSpan(1));
        }

        /// <summary>
        /// Split this string across spaces and find the max number of characters in any one of the split parts.
        /// Null-safe.
        /// </summary>
        /// <returns>0 if this is a null string; otherwise, the character count of the longest substring between spaces</returns>
        public static int LengthOfLongestWord(this string s)
        {
            if (s == null) return 0;

            var words = s.Split(' ');
            return words.Select(x => x.Length).Max();
        }

        /// <summary>
        /// Turn this object into a string by using its default format.
        /// Null-safe. Guaranteed to return a non-null string.
        /// </summary>
        /// <param name="value">any object, or null</param>
        /// <param name="isMoney">is the value expected to represent currency?</param>
        /// <returns>
        /// an empty string if value is null;
        /// the value itself if it's already a string;
        /// value.ToString if there's no default format defined for value's Type;
        /// otherwise, applies string.Format to the value using the determined format
        /// </returns>
        public static string Format(this object value, bool isMoney)
        {
            if (value == null) return "";

            if (value is string s) return s;

            // lookup the default format for the value's Type
            string format = value.GetType()?.GetDefaultFormat(isMoney);

            // get the appropriate string representation of the given value
            string text = (format == null) ? value.ToString()
                : string.Format($"{{0:{format}}}", value);

            return text;
        }

        /// <summary>
        /// Combines parts of a person's name, some of which may or may not be set.
        /// </summary>
        /// <param name="first">First name</param>
        /// <param name="middle">Middle initial/name</param>
        /// <param name="last">Last name</param>
        /// <param name="suffix">Suffix (Jr, III, etc)</param>
        /// <param name="lastNameFirst">Should the formatted name be in a sortable last-name-first style?</param>
        public static string FormatName(string first, string middle, string last, string suffix, bool lastNameFirst)
        {
            StringBuilder sb = new StringBuilder();

            if (lastNameFirst)
            {
                sb.Append(last);
                if (first.HasValue())
                {
                    sb.Append($", {first}");
                }
                if (middle.HasValue())
                {
                    sb.Append($" {middle}");
                }
            }
            else
            {
                sb.Append(first);
                if (middle.HasValue())
                {
                    sb.Append($" {middle}");
                }
                sb.Append($" {last}");
            }

            if (suffix.HasValue())
            {
                sb.Append($" {suffix}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Produces a multi-line string using typical address formatting and the given address parts.
        /// Starts with any number of address lines, which could include name, company, etc.
        /// Then a line like [city], [state] [postalCode].
        /// Then country on the final line.
        /// Any empty parts or empty lines are omitted.
        /// </summary>
        public static string FormatAddress(string city, string state, string postalCode, string country, params string[] addressLines)
        {
            var sb = new StringBuilder();

            var parts = FormatAddressParts(city, state, postalCode, country, addressLines);

            parts.ForEach(x => sb.AppendLine(x));

            return sb.ToString();
        }

        /// <summary>
        /// Produces a multi-line string using typical address formatting and the given address parts.
        /// Starts with any number of address lines, which could include name, company, etc.
        /// Then a line like [city], [state] [postalCode].
        /// Then country on the final line.
        /// Any empty parts or empty lines are omitted.
        /// </summary>
        public static List<string> FormatAddressParts(string city, string state, string postalCode, string country, params string[] addressLines)
        {
            var parts = new List<string>();

            foreach (var line in addressLines)
            {
                if (line.HasValue())
                {
                    parts.Add(line);
                }
            }

            var cityStateZip = new StringBuilder();
            if (city.HasValue())
            {
                cityStateZip.Append($"{city}, ");
            }
            if (state.HasValue())
            {
                cityStateZip.Append($"{state} ");
            }
            if (postalCode.HasValue())
            {
                cityStateZip.Append(postalCode);
            }

            if (cityStateZip.Length > 0)
            {
                parts.Add(cityStateZip.ToString());
            }

            if (country.HasValue())
            {
                parts.Add(country);
            }

            return parts;
        }

        /// <summary>
        /// Get the portion of this string between two character indexes (non-inclusive)
        /// </summary>
        public static string SubstringBetween(this string s, int startPos, int endPos)
        {
            if (s is null) { return null; }

            return s.Substring(startPos, endPos - startPos);
        }

        /// <summary>
        /// Get the number of times the given string appears in this string
        /// </summary>
        public static int CountInstances(this string s, string match)
        {
            return s?.Split(match).Length - 1 ?? 0;
        }

        /// <summary>
        /// Does this string appear to contain html?
        /// </summary>
        public static bool LooksLikeHtml(this string s)
        {
            return s?.Contains("</") ?? false;
        }

        /// <summary>
        /// If this string is longer than the given length, trim it and append an ellipsis
        /// </summary>
        public static string Truncate(this string s, int maxLength)
        {
            if ((s?.Length ?? 0) <= maxLength) { return s; }

            return s.Substring(0, maxLength) + '\u2026';
        }
    }
}
