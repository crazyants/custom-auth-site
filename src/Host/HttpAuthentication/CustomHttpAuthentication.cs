using IdentityServer.Host.Configuration;
using IdentityServer4;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Host.HttpAuthentication
{
    public class CustomHttpAuthentication : ICustomAuthorizeRequestValidator
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IdentityServerOptions _options;

        public CustomHttpAuthentication(IHttpContextAccessor httpContext, IdentityServerOptions options)
        {
            _httpContext = httpContext;
            _options = options;
        }

        public async Task<AuthorizeRequestValidationResult> ValidateAsync(ValidatedAuthorizeRequest request)
        {
            var userId = await _options.HttpLoginCallback?.Invoke(_httpContext.HttpContext);
            
            if (userId != null)
            {
                // this line is informing identityserver who the user is
                request.Subject = IdentityServerPrincipal.Create(userId, userId);
            }

            return new AuthorizeRequestValidationResult
            {
                IsError = false
            };
        }
    }
}
