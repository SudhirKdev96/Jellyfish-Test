using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using WebUI.Data.Enums;
using WebUI.Data.Interfaces;
using WebUI.Data.Models;

namespace WebUI.Data
{
    public class ApplicationUser : IdentityUser, ISelectOption<string>
    {
        // additional, DB mapped fields for users should be defined here
        // create a migration after making changes and then update the database

       
        // not mapped, virtual collections of entities that have many to one relationships with users
        // if you want to use these navigation properties anywhere they need to be defined
        public virtual List<Client> ClientCreatedBy { get; set; }
        public virtual List<Client> ClientChangedBy { get; set; }
        public virtual List<Project> ProjectCreatedBy { get; set; }
        public virtual List<Project> ProjectChangedBy { get; set; }


        /* ISelectOption<string> Implementation */
        public string OptionId => this.Id;
        public string DisplayName => $"{this.UserName}";
    }
}
