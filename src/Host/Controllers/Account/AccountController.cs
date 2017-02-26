// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Host.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityServer4.Quickstart.UI
{
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private readonly IdentityServerOptions _options;
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(
            IdentityServerOptions options,
            IIdentityServerInteractionService interaction)
        {
            _options = options;
            _interaction = interaction;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_options.AllowLocalLogin == false)
            {
                context.Result = new NotFoundResult();
            }
        }

        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // simply check that username is the same as password to authenticate
                if (model.Username == model.Password)
                {
                    // this issues the cookie so the token service knows who the user is
                    await HttpContext.Authentication.SignInAsync(model.Username, model.Username);

                    // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            // something went wrong, show form with error
            return View(model);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var model = new LogoutModel { LogoutId = logoutId };

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (!context.ShowSignoutPrompt)
            {
                // no need to show prompt
                return await Logout(model);
            }

            return View(model);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutModel model)
        {
            // delete local authentication cookie
            await HttpContext.Authentication.SignOutAsync();

            var context = await _interaction.GetLogoutContextAsync(model.LogoutId);

            return View("LoggedOut", new LoggedOutModel
            {
                ClientName = context?.ClientId,
                PostLogoutRedirectUri = context?.PostLogoutRedirectUri,
                SignOutIframeUrl  = context?.SignOutIFrameUrl,
                AutomaticRedirectAfterSignOut = true
            });
        }
    }
}