using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Data;

namespace WebUI.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public class DeleteModel : PageModel
    {
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public DeleteModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ApplicationUser UserToDelete { get; set; }

        public IActionResult OnGet(string userId)
        {
            // get user by id
            var user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user != null)
            {
                UserToDelete = user;
                return Page();
            }
            // that user does not exist... return to list view
            return LocalRedirect("/Identity/Admin/");

        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            // get user by id
            var user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user != null)
            {
                // delete user
                //await _userManager.DeleteAsync(user);

                // disable and lockout user
                //await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                await _userManager.AddToRoleAsync(user, "Disabled");
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                await _userManager.UpdateSecurityStampAsync(user);
            }
            // redirect to list view (index)
            return LocalRedirect("/Identity/Admin/");
        }
    }
}
