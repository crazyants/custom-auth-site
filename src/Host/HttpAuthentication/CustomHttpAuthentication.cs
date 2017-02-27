using IdentityServer4;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using RelativityAuthenticationBridge.Configuration;
using System.Threading.Tasks;

namespace RelativityAuthenticationBridge.HttpAuthentication
{
    public class CustomHttpAuthentication : ICustomAuthorizeRequestValidator
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly RelativityAuthenticationBridgeOptions _options;

        public CustomHttpAuthentication(IHttpContextAccessor httpContext, RelativityAuthenticationBridgeOptions options)
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
                return new AuthorizeRequestValidationResult
                {
                    IsError = false
                };
            }

            // if we can't determine who the user is, we must issue an error
            // since we are not supporing a login page at the same time as 
            // using http-based logins
            return new AuthorizeRequestValidationResult
            {
                IsError = true,
                Error = "authentication_failed"
            };
        }
    }
}
