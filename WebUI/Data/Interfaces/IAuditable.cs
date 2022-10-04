using System;

namespace WebUI.Data.Interfaces
{
    /// <summary>
    /// Defines required elements of a class that saves audit information
    /// </summary>
    public interface IAuditable
    {
        public DateTime CreatedDateTime { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime? ChangedDateTime { get; set; }
        public string? ChangedById { get; set; }
        public ApplicationUser? ChangedBy { get; set; }
    }
}
