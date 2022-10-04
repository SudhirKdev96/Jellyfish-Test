using Microsoft.AspNetCore.Components;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Data.Interfaces;
using WebUI.ViewModels;
using System.Collections.Generic;
using WebUI.Components.Abstract;

namespace WebUI.Components
{
    /// <summary>
    /// Autocomplete component that loads a full set of one entity type
    /// </summary>
    /// <typeparam name="TItem">the type of entity to load. Must implement ISelectOption<int></typeparam>
    public partial class AutocompleteEntity<TItem> : CustomSelectorBase<TItem, ISelectOption<int>, int?> where TItem : class, ISelectOption<int>
    {
        #region Convenience Properties
        private ISelectOption<int> _selectedOption => _options?.Where(o => o.OptionId == Value).FirstOrDefault();
        #endregion


        #region Parameters
        /// <summary>
        /// Markup for displaying content on each line more complicated than just the basic display name
        /// </summary>
        [Parameter]
        public RenderFragment<ISelectOption<int>> ItemTemplate { get; set; }
        #endregion


        /// <summary>
        /// callback when the user picks a new option
        /// </summary>
        protected void SelectedOptionChanged(ISelectOption<int> newOption)
        {
            int? newId = newOption?.OptionId;

            if (Value == newId) return;

            Value = newId;

            if (ValueChanged.HasDelegate)
            {
                ValueChanged.InvokeAsync(Value);
                if (DoChangeNotifications)
                {
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }

                SelectionChanged.InvokeAsync(Value);
            }
        }

        protected override async Task<IEnumerable<ISelectOption<int>>> LoadOptionsAsync()
        {
            var service = await _factory.CreateServiceBaseAsync();
            return await service.GetSelectOptions<TItem, int>(Filter);
        }

        /// <summary>
        /// Get the display name of the selected option, or null if nothing is selected
        /// </summary>
        public string GetSelectedOptionName()
        {
            return _selectedOption?.DisplayName;
        }
    }
}
