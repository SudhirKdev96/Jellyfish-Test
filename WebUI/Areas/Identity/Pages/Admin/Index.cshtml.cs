using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Data;

namespace WebUI.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public partial class IndexModel : PageModel
    {
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IDictionary<ApplicationUser, IList<string>> UserRoleDict { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadUserList()
        {
            UserRoleDict = new Dictionary<ApplicationUser, IList<string>>();

            var users = _userManager.Users;
            foreach (var user in users)
            {
                UserRoleDict.Add(user, await _userManager.GetRolesAsync(user));
            }

            // sort disabled users to the bottom, then by username
            UserRoleDict = UserRoleDict
                .OrderBy(x => x.Value.Any(v => v.Equals("Disabled", StringComparison.CurrentCultureIgnoreCase)))
                .ThenBy(x => x.Key.NormalizedUserName)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            StatusMessage = null;

            await LoadUserList();

            return Page();
        }

        public async Task<IActionResult> OnPostEnableAsync(string userId)
        {
            var user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user != null)
            {
                // clear the disabled role and lockout
                await _userManager.RemoveFromRoleAsync(user, "Disabled");
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
            }

            // redirect to the edit page since the user will need a role assigned
            return LocalRedirect($"/Identity/Admin/Edit/{userId}/");
        }

        public async Task<IActionResult> OnPostDisableAsync(string userId)
        {
            StatusMessage = null;

            // get user by id
            Console.WriteLine("Looking up user");
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                StatusMessage = "Error: User not found";
            }
            else
            {
                // invalidate any active login for the user
                // this needs to be done before anything else, because if security stamp
                // is still null (from an access import) then other operations will fail
                var result = await UpdateIfOk(null, um => um.UpdateSecurityStampAsync(user));

                // remove any existing roles
                var roles = await _userManager.GetRolesAsync(user);
                result = await UpdateIfOk(result, um => um.RemoveFromRolesAsync(user, roles));

                // give user the Disabled role (which really just provides decoration on the user list)
                result = await UpdateIfOk(result, um => um.AddToRoleAsync(user, "Disabled"));

                // lockout a user from logging in
                result = await UpdateIfOk(result, um => um.SetLockoutEnabledAsync(user, true));
                // for ever
                result = await UpdateIfOk(result, um => um.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue));

                if (result.Succeeded)
                {
                    StatusMessage = $"{user.UserName} Disabled";
                }
            }

            await LoadUserList();

            return Page();
        }

        private async Task<IdentityResult> UpdateIfOk(IdentityResult result, Func<UserManager<ApplicationUser>, Task<IdentityResult>> updateOp)
        {
            if (result?.Succeeded ?? true)
            {
                result = await updateOp(_userManager);
            }

            if (!result.Succeeded)
            {
                StatusMessage = $"Error: {result.Errors.First().Description}";
            }

            return result;
        }

        public bool IsAdmin(string roleName)
        {
            return roleName.Contains("admin", System.StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsDisabled(string rolename)
        {
            return rolename.Equals("disabled", System.StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
