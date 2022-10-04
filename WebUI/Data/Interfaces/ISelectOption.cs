using WebUI.ViewModels;

namespace WebUI.Data.Interfaces
{
    /// <summary>
    /// Defines required elements of a class that can be used for options in a select component
    /// </summary>
    public interface ISelectOption<TKey>
    {
        public TKey OptionId { get; }

        public string DisplayName { get; }

        public SelectOption<TKey> ToSelectOption()
        {
            if (this is SelectOption<TKey> o)
            {
                return o;
            }

            return new SelectOption<TKey>
            {
                OptionId = this.OptionId,
                DisplayName = this.DisplayName
            };
        }
    }
}
