using IdentityServer4;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.HttpAuthentication
{
    public class CustomHttpAuthentication : ICustomAuthorizeRequestValidator
    {
        private readonly IHttpContextAccessor _httpContext;

        public CustomHttpAuthentication(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public Task<AuthorizeRequestValidationResult> ValidateAsync(ValidatedAuthorizeRequest request)
        {
            var userId = _httpContext.HttpContext.Request.Headers["userId"].FirstOrDefault() ??
                _httpContext.HttpContext.Request.Query["userId"].FirstOrDefault();
            
            // should we check for anon user if we use the http header?
            if (userId != null && request.Subject.Identity.IsAuthenticated == false)
            {
                // this line is informing identityserver who the user is
                request.Subject = IdentityServerPrincipal.Create(userId, "display name for:" + userId);
            }

            return Task.FromResult(new AuthorizeRequestValidationResult
            {
                IsError = false
            });
        }
    }
}
