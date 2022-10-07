using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Data.Models;
using WebUI.Data.Models.Reports;

namespace WebUI.Services
{
    /// <summary>
    /// Service for KAMS report data
    /// </summary>
    public class CompanyService
    {
        protected readonly ApplicationDbContext<ApplicationUser> context;

        public CompanyService(ApplicationDbContext<ApplicationUser> context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get a single entity of type T.
        /// </summary>
        /// <typeparam name="T">The type of entity to get.</typeparam>
        /// <returns>a Task that returns the entity of type T or null if not found</returns>
        public async Task<T> FindSingleEntity<T>() where T : class
        {
            return await context.Set<T>().FirstOrDefaultAsync();
        }
    }
}
