using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WebUI.Components.Abstract;
using System.Diagnostics;

namespace WebUI.Components
{
    /// <summary>
    /// Autocomplete component that loads a full set of string values for one entity type
    /// </summary>
    /// <typeparam name="TItem">the type of entity to load</typeparam>
    public partial class AutocompleteString<TItem> : CustomSelectorBase<TItem, string, string> where TItem : class
    {
        #region Parameters
        /// <summary>
        /// The expression to the text value / display name of the selected type
        /// </summary>
        [Parameter]  
        public Expression<Func<TItem, string>> TextField { get; set; }

        /// <summary>
        /// Should this component only list distinct options?
        /// </summary>
        [Parameter]
        public bool DistinctValuesOnly { get; set; } = true;
        #endregion


        /// <summary>
        /// callback when the user picks a new option
        /// </summary>
        protected void SelectedOptionChanged(string newOption)
        {
            if (Value == newOption) return;

            Value = newOption;

            if (ValueChanged.HasDelegate)
            {
                Debug.WriteLine($"ACString ValueChanged triggered with new value {newOption}");
                ValueChanged.InvokeAsync(Value);
                if (DoChangeNotifications)
                {
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }

                SelectionChanged.InvokeAsync(Value);
            }
        }


        protected override async Task<IEnumerable<string>> LoadOptionsAsync()
        {
            var service = await _factory.CreateServiceBaseAsync();
            return await service.GetFieldValues<TItem, string>(TextField, DistinctValuesOnly, Filter);
        }
    }
}
