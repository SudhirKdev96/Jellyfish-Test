using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Components.Abstract;
using WebUI.Data;
using WebUI.Data.Interfaces;
using WebUI.Services;

namespace WebUI.Components
{
    /// <summary>
    /// Autocomplete component that loads a full set of string values for one entity type
    /// </summary>
    /// <typeparam name="TUser">the type of user (e.g. TUser, ClientPortalUser)</typeparam>
    /// <typeparam name="TService">the service to use (must be the one that handles TUser)</typeparam>
    public partial class AutocompleteUser<TUser, TService> : CustomSelectorBase<TUser, ISelectOption<string>, string>
        where TUser : IdentityUser, ISelectOption<string>
        where TService : ServiceBase
    {
        #region Convenience Properties
        private ISelectOption<string> _selectedOption => _options?.Where(o => o.OptionId == Value).FirstOrDefault();
        #endregion

        /// <summary>
        /// callback when the user picks a new option
        /// </summary>
        protected void SelectedOptionChanged(ISelectOption<string> newOption)
        {
            string newId = newOption?.OptionId;

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

        protected override async Task<IEnumerable<ISelectOption<string>>> LoadOptionsAsync()
        {
            var service = await _factory.CreateService<TService>();
            return await service.GetSelectOptions<TUser, string>(Filter);
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
