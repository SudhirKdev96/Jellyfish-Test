using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebUI.Data.Models
{
    /// <summary>
    /// Defines extension methods for the AuthenticationState class
    /// </summary>
    public static class AuthenticationStateExtension
    {
        /// <summary>
        /// gets the user Id from an AuthenticationState instance
        /// </summary>
        public static string GetUserId(this AuthenticationState authState)
        {
            // IdentityOptions helps get the user Id from ClaimsPrincipal object
            // based on aspnetcore source: 
            // https://github.com/dotnet/aspnetcore/blob/86a667772feac6516cfb18c1d0d42acff7c4f3ef/src/Identity/Extensions.Core/src/UserManager.cs#L420
            IdentityOptions options = new IdentityOptions();
            string id = authState.User.FindFirstValue(options.ClaimsIdentity.UserIdClaimType);
            return id;
        }

        /// <summary>
        /// checks if a user is in a given role, convenience wrapper around ClaimsPrinciple.IsInRole()
        /// </summary>
        /// <param name="role">the role name to check</param>
        /// <returns>true if the user is in the provided role, false otherwise</returns>
        public static bool IsInRole(this AuthenticationState authState, string role)
        {
            return authState.User.IsInRole(role);
        }
    }
}
