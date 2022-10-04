using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Components.Abstract
{
    /// <summary>
    /// Base class for custom selectors (e.g. Autocompletes, Multi-selection tables) that load data based around DB entities
    /// </summary>
    /// <typeparam name="TItem">The DB entity type</typeparam>
    /// <typeparam name="TOption">The Type that TItem will manipulated into, to be used to populate the ultimate selector component</typeparam>
    /// <typeparam name="TBound">The type used for the @bind-Value attribute where the concrete components are used</typeparam>
    public abstract class CustomSelectorBase<TItem, TOption, TBound> : InputBase<TBound> where TItem : class
    {
        #region Private Fields
        // the set of options the user can select from
        protected IEnumerable<TOption> _options;

        // maintains the last state of this component for deteramining if anything actually changed when the parent re-renders
        private ComponentState _oldState = null;

        // a flag for explicitly marking the current state of this component as invalid
        private bool _stateIsInvalid;

        // is the component in the middle of refreshing its options?
        protected bool IsRefreshing;

        // flag for whether or not this page is still loading for the first time
        protected bool IsFirstLoad = true;
        #endregion


        #region Parameters
        /// <summary>
        /// An optional value to show when the options are loaded, but there's no selection made
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }

        /// <summary>
        /// an optional predicate to filter, order, include, or otherwise manipulate 
        /// the set of options that will populate this component
        /// </summary>
        [Parameter]
        public Func<IQueryable<TItem>, IQueryable<TItem>> Filter { get; set; }

        /// <summary>
        /// prevent this component from loading options from the database itself, this flag should be used whenever OptionsOverride will be passed in
        /// </summary>
        [Parameter]
        public bool UseOptionsOverride { get; set; }

        /// <summary>
        /// bypass the standard loading of options and use this collection instead
        /// </summary>
        [Parameter]
        public IEnumerable<TOption> OptionsOverride { get; set; }

        /// <summary>
        /// bypass the standard loading of options and use this function to populate the component instead
        /// </summary>
        [Parameter]
        public Func<Task<IEnumerable<TOption>>> LoadOptionsOverrideAsync { get; set; }

        /// <summary>
        /// Can the user put an ad-hoc value in this field?
        /// </summary>
        [Parameter]
        public bool AllowCustomValues { get; set; }

        /// <summary>
        /// CSS class for the div containing this component
        /// </summary>
        [Parameter]
        public string ContainerClass { get; set; }

        /// <summary>
        /// CSS class for the component itself
        /// </summary>
        [Parameter]
        public string Class { get; set; } = "form-control form-control-sm";

        /// <summary>
        /// disable this input component?
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }
        #endregion

        /// <summary>
        /// A CSS value for the "width" style parameter
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        /// Should this component fire field change notifications? Defaults to yes
        /// </summary>
        [Parameter]
        public bool DoChangeNotifications { get; set; } = true;

        /// <summary>
        /// optional callback for when the user selects a new item
        /// </summary>
        [Parameter]
        public EventCallback<TBound> SelectionChanged { get; set; }

        /// <summary>
        /// Convenience property for determining if there are any options to display
        /// </summary>
        public bool HasAnyOptions => (_options?.Any() ?? false);

        protected override async Task OnParametersSetAsync()
        {
            // refresh options, if necessary
            if (DoRefreshOptions())
            {
                await RefreshOptionsAsync();
            }

            await base.OnParametersSetAsync();

            IsFirstLoad = false;
        }

        /// <summary>
        /// Is this component in a state where the available options need to be refreshed?
        /// </summary>
        protected virtual bool DoRefreshOptions()
        {
            // compile the current state of this component with the updated parameters
            var currentState = new ComponentState
            {
                Filter = this.Filter,
                LoadOptionsOverride = this.LoadOptionsOverrideAsync,
                OptionsOverride = this.OptionsOverride,
            };

            // if the current state is marked invalid, or if it has changed from the previous state, refresh
            var doRefresh = _stateIsInvalid || !currentState.Equals(_oldState);

            // save the current state for later comparison and clear the invalid state flag
            _oldState = currentState;
            _stateIsInvalid = false;

            return doRefresh;
        }

        /// <summary>
        /// Do the specific work to load the set of options required
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IEnumerable<TOption>> LoadOptionsAsync();


        /// <summary>
        /// Refresh the set of options backing this autocomplete
        /// </summary>
        public async Task RefreshOptionsAsync()
        {
            IsRefreshing = true;

            // set null first so the loading placeholder shows
            _options = null;


            if (OptionsOverride != null || UseOptionsOverride)
            {
                _options = OptionsOverride;
            }
            else if (LoadOptionsOverrideAsync != null)
            {
                _options = await LoadOptionsOverrideAsync();
            }
            else
            {
                _options = await LoadOptionsAsync();
            }

            IsRefreshing = !HasAnyOptions;
        }

        /// <summary>
        /// Explicitly mark the current state of this component invalid, 
        /// forcing it to refresh its options the next time it checks if it needs to.
        /// Necessary if you have a filter whose definition hasn't changed, but the values of its criteria have
        /// </summary>
        public void InvalidateState()
        {
            _stateIsInvalid = true;
            Value = default;
            if (ValueChanged.HasDelegate)
            {
                ValueChanged.InvokeAsync(default);
            }
        }

        /// <summary>
        /// get placeholder text to show in the component in various situations
        /// </summary>
        protected virtual string GetPlaceHolderText()
        {
            if (_options == null) return "[Loading...]";

            if (_options.Count() < 1) return "[No options]";

            return Placeholder;
        }

        /// <summary>
        /// A container for holding the fields of this component that represent its functional state.
        /// Implements IEquatable in order to compare states at different times.
        /// </summary>
        private class ComponentState : IEquatable<ComponentState>
        {
            public Func<IQueryable<TItem>, IQueryable<TItem>> Filter { get; set; }
            public Func<Task<IEnumerable<TOption>>> LoadOptionsOverride { get; set; }
            public IEnumerable<TOption> OptionsOverride { get; set; }

            public bool Equals(ComponentState cs)
            {
                if (cs == null) return false;

                if (Filter == null)
                {
                    if (cs.Filter != null) return false;
                }
                else
                {
                    if (!Filter.Equals(cs.Filter)) return false;
                }

                if (LoadOptionsOverride == null)
                {
                    if (cs.LoadOptionsOverride != null) return false;
                }
                else
                {
                    if (!LoadOptionsOverride.Equals(cs.LoadOptionsOverride)) return false;
                }

                if (OptionsOverride == null)
                {
                    if (cs.OptionsOverride != null) return false;
                }
                else
                {
                    if (!OptionsOverride.Equals(cs.OptionsOverride)) return false;
                }

                return true;
            }
        }

        protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TBound result, [NotNullWhen(false)] out string validationErrorMessage)
        {
            // required implementation for extending InputBase, but irrelevant to our use
            throw new System.NotImplementedException();
        }
    }
}
