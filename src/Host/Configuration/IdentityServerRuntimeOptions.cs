
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Host.Configuration
{
    public class IdentityServerRuntimeOptions
    {
        public string CertificateFileName { get; set; }
        public string CertificatePassword { get; set; }
        public ClientOptions kCuraIdentityServerClientOptions { get; set; }

        internal Client kCuraIdentityServerClient
        {
            get
            {
                return new Client
                {
                    ClientId = kCuraIdentityServerClientOptions.ClientId,
                    ClientName = IdentityServerHostConstants.kCuraIdentityServerClientName,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    RedirectUris = kCuraIdentityServerClientOptions.RedirectUris,
                    PostLogoutRedirectUris = kCuraIdentityServerClientOptions.PostLogoutRedirectUris,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                };
            }
        }
    }

    public class ClientOptions
    {
        public string ClientId { get; set; }
        public ICollection<string> RedirectUris { get; set; }
        public ICollection<string> PostLogoutRedirectUris { get; set; }
        // TODO:kCura discuss
        //public string LogoutUri { get; set; }
    }
}

