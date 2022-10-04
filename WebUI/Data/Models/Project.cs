using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using WebUI.Data.Enums;

namespace WebUI.Data.Models
{
    public partial class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public int? ClientId { get; set; }

        public Client? Client { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(250, ErrorMessage = "Name must be 250 characters or shorter.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [Range(typeof(DateTime), "1970-01-01", "2100-01-01", ErrorMessage = "Start Date must be between 01/01/1970 and 01/01/2100")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [Range(typeof(DateTime), "1970-01-01", "2100-01-01", ErrorMessage = "End Date must be between 01/01/1970 and 01/01/2100")]
        public DateTime? EndDate { get; set; }

        [EnumDataType(typeof(Frequency), ErrorMessage = "Invoice Frequency is required.")]
        public Frequency InvoiceFrequency { get; set; }

        public string? Notes { get; set; }

        [EnumDataType(typeof(BillingType), ErrorMessage = "Billing Type is required.")]
        public BillingType BillingType { get; set; }

        [Required(ErrorMessage = "Billing Rate is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Billing Rate must be between {1} and {2}.")]
        public decimal BillingRate { get; set; }

        [Required(ErrorMessage = "Deposit is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Deposit must be between {1} and {2}.")]
        public decimal Deposit { get; set; }

        [Required(ErrorMessage = "Referral Percent is required.")]
        [Range(0, 100, ErrorMessage = "Referral Percent must be between {1} and {2}.")]
        public decimal ReferralPercent { get; set; }

        [Required(ErrorMessage = "Estimated Revenue is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Estimated Revenue Rate must be between {1} and {2}.")]
        public decimal EstimatedRevenue { get; set; }

        [Required(ErrorMessage = "Active is a required field.")]
        public bool Active { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CreatedById { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public DateTime? ChangedDateTime { get; set; }

        public string? ChangedById { get; set; }

        public ApplicationUser? ChangedBy { get; set; }

        public bool Deleted { get; set; }

        public string? DeletedById { get; set; }

        public ApplicationUser? DeletedBy { get; set; }

        public DateTime? DeletedDateTime { get; set; }

    }
}
