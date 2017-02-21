using Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client
    .Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model.Username == model.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim("sub", model.Username)
                };

                var ci = new ClaimsIdentity(claims, "password", "name", "role");
                var cp = new ClaimsPrincipal(ci);

                // TODO:ba OIDC magic here??

                await HttpContext.Authentication.SignInAsync("Cookies", cp);

                if (model.ReturnUrl != null)
                {
                    return LocalRedirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}

