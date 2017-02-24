// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using IdentityServer.Host.Configuration;
using IdentityServer.Host;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using IdentityServer4.Models;
using Host.HttpAuthentication;
using IdentityServer4.Services;

namespace Host
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IdentityServerRuntimeOptions IdentityServerOptions { get; }

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();

            Configuration = builder.Build();
            
            // TODO:brock cleanup
            IdentityServerOptions = Configuration.Get<IdentityServerRuntimeOptions>(IdentityServerHostConstants.IdentityServerRuntimeOptionsSectionName);

            loggerFactory.ConfigureLogging(Configuration.GetSection(IdentityServerHostConstants.LoggingSectionName));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // TODO:brock move into extension method
            // 2 different APIs for the 2 use cases (ensure that we don't allow local logins for the http-only)
            var builder = services.
                AddIdentityServer(ids =>
                {
                    // TODO:brock config all of these
                    ids.Endpoints.EnableEndSessionEndpoint = false;

                    // this lets IdentityServer know the cookie middleware that was used on the login page
                    ids.Authentication.AuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                })
                .AddInMemoryClients(new Client[] {
                    IdentityServerOptions.RelativityClient
                })
                .AddInMemoryIdentityResources(new IdentityResource[] {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                })
                // TODO:brock won't do both -- do extension methods for the two diff scenario
                .AddCustomAuthorizeRequestValidator<CustomHttpAuthentication>();
            services.AddSingleton<IClientSessionService, NopClientSessionService>();

            // TODO:brock move to extension method
            if (IdentityServerOptions.CertificateFileName != null)
            {
                var signingCertificate = new X509Certificate2(IdentityServerOptions.CertificateFileName, IdentityServerOptions.CertificatePassword);
                builder.AddSigningCredential(signingCertificate);
            }
            else if (IdentityServerOptions.CertificateStoreSubjectDistinguishedName != null)
            {
                builder.AddSigningCredential(IdentityServerOptions.CertificateStoreSubjectDistinguishedName);
            }
            else
            {
                // TODO:brock document this is temp
                builder.AddTemporarySigningCredential();
            }

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // TODO:brock leave but comment re: lifetime, etc
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme,
            });

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}