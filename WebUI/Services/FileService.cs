using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace WebUI.Services
{
    /// <summary>
    /// Service for viewing and saving files
    /// </summary>
    public class FileService
    {
        protected IJSRuntime _jsRuntime;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsRuntime"></param>
        public FileService(IJSRuntime jsRuntime) 
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Take file contents as a base-64 string and trigger a download to save it as a file
        /// </summary>
        /// <param name="content">a base-64 string representing content to be saved</param>
        /// <param name="fileName">the default name of the file download</param>
        private async Task Download(string content, string fileName)
        {
            await _jsRuntime.InvokeVoidAsync("saveAsFile", fileName, content);
        }

        /// <summary>
        /// Take file contents as a UTF-8 byte array and trigger a download to save it as a file
        /// </summary>
        /// <param name="content">a UTF-8 byte array representing content to be saved</param>
        /// <param name="fileName">the default name of the file download</param>
        public async Task Download(byte[] content, string fileName)
        {
            await Download(Convert.ToBase64String(content), fileName);
        }

        /// <summary>
        /// Take file contents as a base-64 string and open it in a new tab.
        /// NOTE: Will fail for particularly large documents.
        /// <param name="content">a base-64 string representing content to be shown</param>
        /// </summary>
        public async Task OpenInNewTab(string content)
        {
            await _jsRuntime.InvokeVoidAsync("OpenIntoNewTab", content);
        }
    }
}