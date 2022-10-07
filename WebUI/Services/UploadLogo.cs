using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Data.Enums;
using WebUI.Data.Models;

namespace WebUI.Services
{
    public class UploadLogo : IDisposable
    {
        protected readonly ApplicationDbContext<ApplicationUser> context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public event Action<string,string> OnShow;

        public UploadLogo(ApplicationDbContext<ApplicationUser> context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public void UpdateLogo(string url, string title)
        {
            OnShow?.Invoke(url, title);
        }

        public string GetFaviconUrl()
        {
            var companyProfile = context.Set<CompanyProfile>().FirstOrDefault() ?? new CompanyProfile();
            var basePath = _webHostEnvironment.WebRootPath;
            if (companyProfile.Favicon is not null && !string.IsNullOrEmpty(companyProfile.Favicon) && System.IO.File.Exists(basePath + EnumExtension.GetDescription(FolderPath.CompanyLogo) + companyProfile.Logo)) 
            {
                byte[] byteArray = System.IO.File.ReadAllBytes(basePath + EnumExtension.GetDescription(FolderPath.Favicon) + companyProfile.Favicon);
                return Convert.ToBase64String(byteArray);
            }
            return string.Empty;
        }

        public void Dispose()
        {
        }
    }
}
