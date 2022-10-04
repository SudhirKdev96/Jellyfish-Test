using iTextSharp.text;

namespace WebUI.Pdf
{
    /// <summary>
    /// A model for options related to the overall PDF document (not relative to a specific data set)
    /// </summary>
    public class PdfDocumentOptions
    {
        /// <summary>
        /// default font for the repeating document header
        /// </summary>
        public static readonly Font DEFAULT_HEADER_FONT = new Font(Font.TIMES_ROMAN, 18, Font.BOLD, BaseColor.Black);

        /// <summary>
        /// default font for the repeating document footer
        /// </summary>
        public static readonly Font DEFAULT_FOOTER_FONT = new Font(Font.TIMES_ROMAN, 10, Font.BOLD, BaseColor.Black);

        /// <summary>
        /// The font in which to render the repeating document header
        /// </summary>
        public Font HeaderFont { get; set; } = DEFAULT_HEADER_FONT;

        /// <summary>
        /// The font in which to render the repeating document footer
        /// </summary>
        public Font FooterFont { get; set; } = DEFAULT_FOOTER_FONT;

        /// <summary>
        /// The title of this document, shown as a repeating header
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Orient this document in landscape instead of the default portrait
        /// </summary>
        public bool UseLandscapeOrientation { get; set; } = false;

        /// <summary>
        /// the size of the pages in this report. Defaults to Letter if null
        /// (suggested values are the constants from PageSize, e.g. PageSize.Legal)
        /// </summary>
        public Rectangle PaperSize { get; set; } = PageSize.Letter;

        /// <summary>
        /// Text to be included in the footer along with page number and current date
        /// </summary>
        public string FooterText { get; set; }

        /// <summary>
        /// Takes the current header font and changes its size
        /// </summary>
        public void SetHeaderFontSize(float newSize)
        {
            Font f = new Font(HeaderFont)
            {
                Size = newSize
            };

            HeaderFont = f;
        }
    }
}
