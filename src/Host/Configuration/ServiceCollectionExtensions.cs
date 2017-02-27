using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using RelativityAuthenticationBridge;
using RelativityAuthenticationBridge.Configuration;
using RelativityAuthenticationBridge.HttpAuthentication;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        static Client CrateRelativityClient(RelativityAuthenticationBridgeOptions options)
        {
            return new Client
            {
                ClientId = options.RelativityClientId,
                ClientName = RelativityAuthenticationBridgeConstants.RelativityClientName,
                AllowedGrantTypes = GrantTypes.Implicit,
                RequireConsent = false,
                RedirectUris = options.RelativityRedirectUris,
                PostLogoutRedirectUris = options.RelativityPostLogoutRedirectUris,
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
            };
        }

        public static IServiceCollection AddRelativityAuthenticationBridge(this IServiceCollection services, RelativityAuthenticationBridgeOptions options)
        {
            services.AddSingleton(options);

            var builder = services.
                AddIdentityServer(ids =>
                {
                    // if we have cookies for signin, then we should allow signout
                    ids.Endpoints.EnableEndSessionEndpoint = options.AllowLocalLogin;
                    
                    // these endpoints are not used in this scenario
                    ids.Endpoints.EnableCheckSessionEndpoint = false;
                    ids.Endpoints.EnableIntrospectionEndpoint = false;
                    ids.Endpoints.EnableTokenEndpoint = false;
                    ids.Endpoints.EnableTokenRevocationEndpoint = false;
                    ids.Endpoints.EnableUserInfoEndpoint = false;

                    // this lets IdentityServer know the cookie middleware that was used on the login page
                    ids.Authentication.AuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                })
                .AddInMemoryClients(new Client[] { CrateRelativityClient(options) })
                .AddInMemoryIdentityResources(new IdentityResource[] {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                });

            if (options.CertificateStoreSubjectDistinguishedName != null)
            {
                builder.AddSigningCredential(options.CertificateStoreSubjectDistinguishedName);
            }
            else if (options.CertificateFileName != null)
            {
                var signingCertificate = new X509Certificate2(options.CertificateFileName, options.CertificatePassword);
                builder.AddSigningCredential(signingCertificate);
            }
            else
            {
                // if you have not configured a certificate that contains the signing key, then this will allow
                // for a dynamically created key to be used. the only concern is that a new key will be created
                // each time the server is restarted. this option is not feasible for use in production scenarios.
                builder.AddTemporarySigningCredential();
            }

            if (options.HttpLoginCallback != null)
            {
                builder.AddCustomAuthorizeRequestValidator<CustomHttpAuthentication>();
                services.AddSingleton<IClientSessionService, NopClientSessionService>();
            }

            return services;
        }
    }
}
