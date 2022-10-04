using System.Collections.Generic;
using WebUI.ViewModels;

namespace WebUI.Data.Interfaces
{
    /// <summary>
    /// Defines required elements of a class that can be used for options in a select component
    /// with the displayed text intended to be formatted
    /// </summary>
    public interface IFormattedSelectOption<TKey> : ISelectOption<TKey>
    {
        public IEnumerable<string> DisplayParts { get; }

        public FormattedSelectOption<TKey> ToFormattedSelectOption()
        {
            if (this is FormattedSelectOption<TKey> o)
            {
                return o;
            }

            return new FormattedSelectOption<TKey>
            {
                OptionId = this.OptionId,
                DisplayName = this.DisplayName,
                DisplayParts = this.DisplayParts
            };
        }
    }
}
