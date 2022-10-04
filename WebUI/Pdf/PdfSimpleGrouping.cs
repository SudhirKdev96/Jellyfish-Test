namespace WebUI.Pdf
{
    /// <summary>
    /// A way to provide a single group heading for all the data under it
    /// (as opposed to the standard grouping that breaks up the data into subsections)
    /// </summary>
    public class PdfSimpleGrouping<T> : PdfGrouping<T>
    {
        /// <summary>
        /// Construct a new simple grouping with the given heading text
        /// </summary>
        /// <param name="heading">the group heading text</param>
        public PdfSimpleGrouping(string heading) : base(x => heading) { }
    }
}
