using System.Security.Cryptography.X509Certificates;
using IdentityServer.Configuration;
using IdentityServer4;
//using IdentityServer;
//using IdentityServer.Authentication;
//using IdentityServer.Registration;
//using IdentityServer.Stores;
//using IdentityServer.Utility;
//using IdentityServer.Admin;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IIdentityServerBuilder AddkCuraIdentityServer(
            this IServiceCollection services,
            IdentityServerRuntimeOptions options)
        {
            services.AddSingleton(options);

            /*  TODO:ba  - do we need to pull all this in as well?
             *            if (options.UseConfigurationStore)
                        {
                            services.AddTransient<IClientStore, ClientStore>();
                            services.AddTransient<ICorsPolicyService, CorsPolicyService>();
                            services.AddTransient<IResourceStore, ResourceStore>();
                            services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
                        }

                        services.AddTransient<AdminClient>();
                        services.AddTransient<RegistrationClient>();
                        services.AddTransient<AuthenticationClient>();
                        services.AddTransient<AuthenticatedHttpClientFactory>();
                        */
            var builder = services.
                AddIdentityServer(ids =>
                {
                    ids.Authentication.AuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                    ids.Discovery.ShowClaims = false;
                    ids.Discovery.ShowIdentityScopes = false;
                    ids.Discovery.ShowApiScopes = false;
                });
//                .AddProfileService<ProfileService>();
            
            if (options.CertificateFileName != null)
            {
                var signingCertificate = new X509Certificate2(options.CertificateFileName, options.CertificatePassword);
                builder.AddSigningCredential(signingCertificate);
            }
            else
            {
                builder.AddTemporarySigningCredential();
            }            

            return builder;
        }

        public static IIdentityServerBuilder AddRuntimeData(
            this IIdentityServerBuilder builder,
            IdentityServerRuntimeData options)
        {
            builder.AddInMemoryClients(options.Clients);
            builder.AddInMemoryIdentityResources(options.IdentityResources);
            builder.AddInMemoryApiResources(options.ApiResources);

            return builder;
        }
    }
}
