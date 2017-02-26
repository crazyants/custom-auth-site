using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }

            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "oidc");
        }

        public IActionResult Logout()
        {
            var url = Url.Action("Index", "Home");
            // remove "oidc" if only using http-based signin
            return SignOut(new AuthenticationProperties { RedirectUri = url }, "oidc", "Cookies");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}

