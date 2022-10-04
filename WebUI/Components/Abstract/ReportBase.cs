using Microsoft.AspNetCore.Components;
using WebUI.Factory;
using WebUI.Services;

namespace WebUI.Components
{
    /// <summary>
    /// Base class for report pages with a handful of common necessities
    /// </summary>
    public abstract class ReportBase : ComponentBase
    {
        /// <summary>
        /// Service factory instance, is injected by the service provider container
        /// </summary>
        [Inject]
        private ServiceFactory _factory { get; set; }

        // service instance for report data
        private ReportService _reportService;

        /// <summary>
        /// Service instance for report data.
        /// Waits to instantiate until the first time it's needed.
        /// </summary>
        protected ReportService Service => _reportService ??= _factory.CreateReportService();
    }
}
