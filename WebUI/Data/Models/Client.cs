using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Data.Models
{
    public partial class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string? ShortName { get; set; }

        public string? PaymentTerms { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string StreetAddress { get; set; }

        public string? StreetAddress2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal Code is required.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Phone Number is not a valid.")]
        public string PhoneNumber { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "Billing Rate is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Billing Rate must be between {1} and {2}.")]
        public decimal BillingRate { get; set; }

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
