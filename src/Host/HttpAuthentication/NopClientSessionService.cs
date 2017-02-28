// © 2017, kCura LLC 

using IdentityServer4.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RelativityAuthenticationBridge.HttpAuthentication
{
    // nop implementation because for http-based scenarios we don't have a cookie
    // the default IClientSessionService assumes a cookie
    public class NopClientSessionService : IClientSessionService
    {
        public Task AddClientIdAsync(string clientId)
        {
            return Task.FromResult(0);
        }

        public Task EnsureClientListCookieAsync(string sid)
        {
            return Task.FromResult(0);
        }

        public Task<IEnumerable<string>> GetClientListAsync()
        {
            return Task.FromResult(Enumerable.Empty<string>());
        }

        public IEnumerable<string> GetClientListFromCookie(string sid)
        {
            return Enumerable.Empty<string>();
        }

        public void RemoveCookie(string sid)
        {
        }
    }
}
