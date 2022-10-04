using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebUI.Data.Interfaces;
using WebUI.ViewModels;
using System.Collections.Generic;
using WebUI.Components.Abstract;
using BlazorTable;
using System.Linq;

namespace WebUI.Components
{
    /// <summary>
    /// Multiselect table component that loads a full set of one entity type
    /// </summary>
    /// <typeparam name="TItem">the type of entity to load. Must implement ISelectOption<int></typeparam>
    public partial class TableSelectEntity<TItem> : CustomSelectorBase<TItem, SelectOption<int>, List<int>> where TItem : class, ISelectOption<int>
    {
        #region Convenience Properties

        // the set of options that the user has selected
        private List<SelectOption<int>> _selectedItems = new List<SelectOption<int>>();

        // make the selected items available to the outside as a read-only collection
        public IReadOnlyCollection<SelectOption<int>> SelectedItems => _selectedItems.AsReadOnly();

        public List<int> SelectedIds => SelectedItems.Select(x => x.OptionId).ToList();
        #endregion


        #region Parameters
        /// <summary>
        /// The max number of rows visible on each page of the table
        /// </summary>
        [Parameter]
        public int PageSize { get; set; }

        /// <summary>
        /// Declares how many rows the user can select at a time (0, 1, or multiple)
        /// </summary>
        [Parameter]
        public SelectionType SelectionType { get; set; } = SelectionType.Multiple;
        #endregion

        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            // Note: binding doesn't work with this component (yet) because BlazorTable just 
            // calls Add and Remove on _selectedItems and we'd need to write something that 
            // extends List and fires with changes on those methods.
            ValueExpression = () => SelectedIds;

            return base.SetParametersAsync(ParameterView.Empty);
        }

        protected override async Task<IEnumerable<SelectOption<int>>> LoadOptionsAsync()
        {
            var service = await _factory.CreateServiceBaseAsync();
            return await service.GetSelectOptions<TItem, int>(Filter);
        }
    }
}
