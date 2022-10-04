using System;
using System.Collections.Generic;
using System.Numerics;

namespace WebUI.Data.Models
{
    public static class TypeExtension
    {
        /// <summary>
        /// All numeric data types in the framework
        /// </summary>
        public static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(byte),
            typeof(sbyte),

            typeof(short),
            typeof(int),
            typeof(long),
 
            typeof(ushort),
            typeof(uint),
            typeof(ulong),

            typeof(decimal),
            typeof(double),
            typeof(float),

            typeof(BigInteger)
        };

        /// <summary>
        /// Does this type represent a number?
        /// (including Nullable numeric types)
        /// </summary>
        public static bool IsNumeric(this Type t)
        {
            if (t == null) return false;

            // compiled from multiple answers here: https://stackoverflow.com/q/1749966
            return NumericTypes.Contains(Nullable.GetUnderlyingType(t) ?? t);
        }

        /// <summary>
        /// Does this object represent a number?
        /// </summary>
        public static bool IsNumeric(this object t)
        {
            if (t == null) return false;

            return t.GetType().IsNumeric();
        }

        /// <summary>
        /// Get the default format for turning an item of this type into a string
        /// </summary>
        /// <param name="isMoney">true to return the money-specific format string</param>
        /// <returns>a string for use in String.Format(..) and elsewhere to convert 
        /// items of the given format into string</returns>
        public static string GetDefaultFormat(this Type type, bool isMoney)
        {
            // if marked as money, return the currency format (to 2 decimal places)
            if (isMoney) return "c2";

            // for dates, default to MM/dd/yyyy
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return "MM/dd/yy";
            }

            // use the "n" format for numeric types to get commas et al
            if (type.IsNumeric())
            {
                return "n";
            }

            // nothing applicable by default
            return null;
        }
    }
}
