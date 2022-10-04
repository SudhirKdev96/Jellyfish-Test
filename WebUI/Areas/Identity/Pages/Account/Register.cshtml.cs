using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebUI.Areas.Identity.Pages.Account
{
    // Register Functionality is Disabled
    [Authorize]
    public class RegisterModel : PageModel
    {
        public void OnGet(string returnUrl = null)
        {
        }
    }
}
