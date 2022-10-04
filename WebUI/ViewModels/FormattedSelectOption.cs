using System.Collections.Generic;
using WebUI.Data.Interfaces;

namespace WebUI.ViewModels
{    
    /// <summary>
    /// Barebones implementation of <c>IFormattedSelectOption</c> for use in selects and autocompletes
    /// </summary>
    public class FormattedSelectOption<TKey> : SelectOption<TKey>, IFormattedSelectOption<TKey>
    {
        public IEnumerable<string> DisplayParts { get; set; }
    }
}
