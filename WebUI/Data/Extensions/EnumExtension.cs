using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace WebUI.Data.Models
{
    /// <summary>
    /// Defines extension methods for all Enums
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Pretty-prints an enum name in Title case with spaces instead of underscores
        /// </summary>
        public static string ToLabel(this Enum e)
        {
            if (e == null) return null;

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            return textInfo.ToTitleCase(e.ToString().ToLower().Replace('_', ' '));
        }

        /// <summary>
        /// Get enum description
        /// </summary>
        public static string GetDescription(System.Enum @enum)
        {
            if (@enum == null)
                return null;

            string description = @enum.ToString();
            try
            {
                FieldInfo fi = @enum.GetType().GetField(@enum.ToString());

                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                    description = attributes[0].Description;
            }
            catch (Exception)
            {
                // _logger.ErrorException("Error in GetDescription Method", ex);
                throw;
            }
            return description;
        }
    }
}
