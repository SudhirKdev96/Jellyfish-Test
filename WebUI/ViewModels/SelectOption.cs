using WebUI.Data.Interfaces;

namespace WebUI.ViewModels
{    
    /// <summary>
    /// Barebones implementation of <c>IselectOption</c> for use in selects and autocompletes
    /// </summary>
    public class SelectOption<TKey> : ISelectOption<TKey>
    {
        public TKey OptionId { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return $"{OptionId.ToString()}: {DisplayName}";
        }
    }
}
