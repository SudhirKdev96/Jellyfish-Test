using System.ComponentModel.DataAnnotations.Schema;
using WebUI.Data.Interfaces;

namespace WebUI.Data.Models
{
    // extension for class Project - use to add non-mapped properties, methods, etc.
    // validation annotations (e.g. [Required]) should NOT be used here as they will not be respected by Blazor
    // when using a code-first approach this extension isn't really necessary
    // when using a database-first approach, the main class definition under Data/Models will be overwritten by EF
    public partial class Project: IAuditable, ISoftDeletable, ISelectOption<int>
    {
        // ISelectOption Implementation - not mapped to database fields
        [NotMapped]
        public int OptionId => ProjectId;

        [NotMapped]
        public string DisplayName => Name;

        // Nullable decimal fields needed for form components (e.g. InputCurrency).
        [NotMapped]
        public decimal? NullableBillingRate 
        { 
            get => BillingRate;
            set
            {
                BillingRate = value ?? 0;
            } 
        }

        [NotMapped]
        public decimal? NullableDeposit
        {
            get => Deposit;
            set
            {
                Deposit = value ?? 0;
            }
        }

        [NotMapped]
        public decimal? NullableEstimatedRevenue
        {
            get => EstimatedRevenue;
            set
            {
                EstimatedRevenue = value ?? 0;
            }
        }
    }
}
