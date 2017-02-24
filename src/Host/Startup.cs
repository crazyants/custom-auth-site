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
            
            // TODO:kCura discuss config class and section names
            IdentityServerOptions = Configuration.Get<IdentityServerRuntimeOptions>(IdentityServerHostConstants.IdentityServerRuntimeOptionsSectionName);

            loggerFactory.ConfigureLogging(Configuration.GetSection(IdentityServerHostConstants.LoggingSectionName));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.
                AddIdentityServer(ids =>
                {
                    // TODO:kCura discuss
                    ids.Authentication.AuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                })
                .AddInMemoryClients(new Client[] {
                    IdentityServerOptions.kCuraIdentityServerClient
                })
                .AddInMemoryIdentityResources(new IdentityResource[] {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                })
                // TODO:kCura discuss if this should be config-based?
                .AddCustomAuthorizeRequestValidator<CustomHttpAuthentication>();
            // TODO:kCura discuss flag to toggle mode: UI vs HTTP
            services.AddSingleton<IClientSessionService, NopClientSessionService>();

            // TODO:kCura discuss
            if (IdentityServerOptions.CertificateFileName != null)
            {
                var signingCertificate = new X509Certificate2(IdentityServerOptions.CertificateFileName, IdentityServerOptions.CertificatePassword);
                builder.AddSigningCredential(signingCertificate);
            }
            else
            {
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

            // TODO:kCura discuss
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