using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Data;

namespace WebUI.Areas.Identity.Pages.Admin
{
    // Take careful consideration before making this less restrictive, since access to
    // this page allows changing a user's password without knowing the current password
    [Authorize(Roles = "SuperAdmin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<EditModel> _logger;
        private readonly IEmailSender _emailSender;

        public EditModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<EditModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Role { get; set; }
        public IList<IdentityRole> RoleList { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string PasswordStatusMessage { get; set; }

        [TempData]
        public string RoleStatusMessage { get; set; }
        
        [TempData]
        public string EmailStatusMessage { get; set; }

        public class InputModel
        {
            // password section
            //[DataType(DataType.Password)]
            //[Display(Name = "Current password")]
            //public string OldPassword { get; set; }

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string? NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }

            // email section
            [EmailAddress]
            [Display(Name = "Current Email")]
            public string? CurrentEmail { get; set; }

            // email section
            [EmailAddress]
            [Display(Name = "New email")]
            public string? NewEmail { get; set; }

            [Display(Name = "Role")]
            public string? NewRole { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user != null)
            {
                Email = user.Email;
                EmailConfirmed = user.EmailConfirmed;
                var userRoles = await _userManager.GetRolesAsync(user);
                Role = userRoles.FirstOrDefault();
                RoleList = _roleManager.Roles.ToList();

                return Page();
            }
            else
            {
                return NotFound($"Unable to load user with Id '{userId}'.");
            }
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user == null)
            {
                return NotFound($"Unable to load user with Email '{Input.CurrentEmail}'.");
            }
            else
            {
                // in case the page needs to be rendered again due to errors (e.g. wrong password)
                Email = user.Email;
                EmailConfirmed = user.EmailConfirmed;
                var userRoles = await _userManager.GetRolesAsync(user);
                Role = userRoles.FirstOrDefault();
                RoleList = _roleManager.Roles.ToList();
            }

            //PasswordStatusMessage = "Password has not been changed.";
            //EmailStatusMessage = "Email has not been changed.";

            // update password if the password form has been submitted
            if (Input.NewPassword != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                var changePasswordResult = await _userManager.ResetPasswordAsync(user, resetToken, Input.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
                else
                {
                    _logger.LogInformation("Admin changed a users password successfully.");
                    PasswordStatusMessage = "Password Updated Successfully.";
                }
            }

            // update email if the email field has been submitted
            if (!string.IsNullOrEmpty(Input.NewEmail))
            {
                if (Input.NewEmail != user.Email)
                {
                    user.Email = Input.NewEmail;
                    user.UserName = Input.NewEmail;
                    user.EmailConfirmed = true;
                    //await _userManager.SetEmailAsync(user, Input.NewEmail);
                    //await _userManager.SetUserNameAsync(user, Input.NewEmail);
                    if (user.SecurityStamp is null)
                    {
                        // Cleanup for generated or imported users that haven't logged in yet
                        await _userManager.UpdateSecurityStampAsync(user);
                    }
                    else
                    {
                        // UpdateSecurityStamp calls this; no need to do it twice
                        await _userManager.UpdateAsync(user);
                    }
                    _logger.LogInformation("Admin changed a users email successfully.");
                    EmailStatusMessage = "Email Updated Successfully.";

                    //var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmailChange",
                    //    pageHandler: null,
                    //    values: new { userId = userId, email = Input.NewEmail, code = code },
                    //    protocol: Request.Scheme);
                    //await _emailSender.SendEmailAsync(
                    //    Input.NewEmail,
                    //    "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //EmailStatusMessage = "Confirmation link to change email sent to user";
                }
            }

            // get current role, and update user role if changed
            var oldRoles = await _userManager.GetRolesAsync(user);
            string oldRole = oldRoles.FirstOrDefault();
            if (Input.NewRole != oldRole)
            {
                // if new role is not empty, proceed to remove and add user to roles
                if (!string.IsNullOrWhiteSpace(Input.NewRole))
                {
                    // if old role is not empty, remove user from role
                    if (!string.IsNullOrWhiteSpace(oldRole))
                    {
                        await _userManager.RemoveFromRoleAsync(user, oldRole);
                    }

                    await _userManager.AddToRoleAsync(user, Input.NewRole);
                    _logger.LogInformation("Admin changed a users role successfully.");
                    RoleStatusMessage = "Role Updated Successfully.";
                }
            }

            return RedirectToPage();
        }

    }
}
