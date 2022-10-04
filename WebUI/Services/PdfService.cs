using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebUI.Pdf;

namespace WebUI.Services
{
    /// <summary>
    /// Service for generating PDF files
    /// </summary>
    public class PdfService : FileService
    {
        public PdfService(IJSRuntime jsRuntime) : base(jsRuntime) { }

        /// <summary>
        /// Start a new PDF document with the given options.
        /// Don't forget to wrap the returned PDF in a using block, or call Dispose() on it manually
        /// </summary>
        /// 
        /// <param name="docOptions">options to override defaults related to the overall PDF document</param>
        /// 
        /// <returns>a PDF document that is open and ready to add data sets onto</returns>
        public PdfBuilder StartPdf(PdfDocumentOptions docOptions)
        {
            try
            {
                // Set the standard company text in the footer
                if (docOptions != null && docOptions.FooterText == null)
                {
                    // TODO: customize document footer by changing "Blazor App" below
                    docOptions.FooterText = "Blazor App";
                }

                return new PdfBuilder(docOptions);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"PdfService.StartPdf: {e.Message}");
                throw e;
            }
        }


        /// <summary>
        /// Start a new PDF document with the given title.
        /// Don't forget to wrap the returned PDF in a using block, or call Dispose() on it manually
        /// </summary>
        /// 
        /// <param name="title">the title of this document, shown as a repeating header.
        /// Uses default Letter size paper in portrait orientation, with default header and footer fonts.</param>
        /// 
        /// <returns>a PDF document that is open and ready to add data sets onto</returns>
        public PdfBuilder StartPdf(string title)
        {
            var docOptions = new PdfDocumentOptions() { Title = title };
            return StartPdf(docOptions);
        }


        /// <summary>
        /// Get output from the given PDF builder and trigger a download to save it as a file
        /// </summary>
        public async Task Download(PdfBuilder pdfBuilder, string fileName)
        {
            await Download(pdfBuilder.GetBytes(), fileName);
        }

        /// <summary>
        /// Get output from the given PDF builder and open it in a new tab.
        /// NOTE: Will fail for particularly large documents.
        /// </summary>
        public async Task OpenInNewTab(PdfBuilder pdfBuilder)
        {
            await OpenInNewTab(pdfBuilder.GetBase64String());
        }
    }
}