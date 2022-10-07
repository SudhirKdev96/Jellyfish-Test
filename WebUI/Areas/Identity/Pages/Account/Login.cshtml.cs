using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Data.Enums;
using WebUI.Data.Models;
using WebUI.Factory;
using WebUI.Services;

namespace WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ServiceFactory _factory;
        public CompanyService Service { get; set; }

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager, ServiceFactory factory, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _factory = factory;
            _webHostEnvironment = webHostEnvironment;
        }

        [ViewData]
        public string LogoUrl { get; set; }
        [ViewData]
        public string SiteTitle { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = "/accounts/")
        {
            Service = Service ?? _factory.CreateCompanyService();
            var companyProfile = await Service.FindSingleEntity<CompanyProfile>() ?? new CompanyProfile();
            var basePath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(companyProfile.Logo) && System.IO.File.Exists(basePath + EnumExtension.GetDescription(FolderPath.CompanyLogo) + companyProfile.Logo))
            {
                byte[] byteArray = System.IO.File.ReadAllBytes(basePath + EnumExtension.GetDescription(FolderPath.CompanyLogo) + companyProfile.Logo);
                LogoUrl = Convert.ToBase64String(byteArray);
            }
            SiteTitle = companyProfile.SiteTitle;

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/accounts/")
        {
            returnUrl = returnUrl == "/" ? "/accounts/" : returnUrl;

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
