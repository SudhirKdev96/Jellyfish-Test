using System;
using System.Diagnostics;
using System.Globalization;

namespace WebUI.Data.Models
{
    /// <summary>
    /// Defines extension methods for all Exceptions
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Drills down to the innermost nested exception and writes its message to the debug output.
        /// </summary>
        /// <param name="preface">optional text to print before the exception message</param>
        public static void DebugOriginalMessage(this Exception e, string preface = null)
        {
            if (e == null) return;

            var ex = e;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            if (preface == null)
            {
                Debug.WriteLine(ex.Message);
            }
            else
            {
                Debug.WriteLine($"{preface} : {ex.Message}");
            }
        }
    }
}
