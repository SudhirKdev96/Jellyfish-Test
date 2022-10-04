using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Data.Interfaces
{
    /// <summary>
    /// Defines required elements of a class that saves delete information
    /// </summary>
    public interface ISoftDeletable : IAuditable
    {
        public DateTime? DeletedDateTime { get; set; }
        public string? DeletedById { get; set; }
        public ApplicationUser? DeletedBy { get; set; }
        public bool Deleted { get; set; }
    }
}

