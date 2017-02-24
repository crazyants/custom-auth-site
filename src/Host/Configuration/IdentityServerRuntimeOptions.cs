
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Host.Configuration
{
    // TODO:brock rebrand to kcura's name
    public class IdentityServerRuntimeOptions
    {
        public string CertificateStoreSubjectDistinguishedName { get; set; }
        public string CertificateFileName { get; set; }
        public string CertificatePassword { get; set; }

        public string RelativityClientId { get; set; }
        public ICollection<string> RelativityRedirectUris { get; set; }

        internal Client RelativityClient
        {
            get
            {
                return new Client
                {
                    ClientId = RelativityClientId,
                    ClientName = IdentityServerHostConstants.kCuraIdentityServerClientName,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    RedirectUris = RelativityRedirectUris,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                };
            }
        }
    }
}

