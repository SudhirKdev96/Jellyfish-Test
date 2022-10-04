using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Data.Enums;
using WebUI.Factory;
using WebUI.Services;

namespace WebUI.Components
{
    public abstract class BasePage : ComponentBase
    {
        /// <summary>
        /// Service factory instance, is injected by the service provider container
        /// </summary>
        [Inject]
        private ServiceFactory _factory { get; set; }

        // service instance for accessing the database
        private ServiceBase _service;

        // the current user
        private ApplicationUser _user;

        /// <summary>
        /// flag for whether or not this page is still loading for the first time
        /// </summary>
        protected bool IsFirstLoad = true;


        //// METHODS ////

        protected async override Task OnInitializedAsync()
        {
            // initialize the service for DB access
            _service = await _factory.CreateServiceBaseAsync();

            // fetch the current user
            _user = _service?.GetCurrentUser();

            if (_user == null)
            {
                throw new Exception("Failed to get the current user");
            }

            await base.OnInitializedAsync();
        }

        protected async override Task OnParametersSetAsync()
        {
            // load in the last selected user values when this page is first loading
            if (IsFirstLoad)
            {
                IsFirstLoad = false;
            }

            await base.OnParametersSetAsync();
        }
    }
}
