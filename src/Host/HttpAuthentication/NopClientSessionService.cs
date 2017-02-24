using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.HttpAuthentication
{
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
