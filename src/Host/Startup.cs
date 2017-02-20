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

namespace Host
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IdentityServerRuntimeOptions IdentityServerOptions { get; }
//        public IdentityServerRuntimeData IdentityServerRuntimeData { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();

            Configuration = builder.Build();
            IdentityServerOptions = Configuration.Get<IdentityServerRuntimeOptions>(IdentityServerHostConstants.IdentityServerRuntimeOptionsSectionName);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(IdentityServerOptions);

            var builder = services.
                AddIdentityServer(ids =>
                {
                    ids.Authentication.AuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
                    ids.Discovery.ShowClaims = false;
                    ids.Discovery.ShowIdentityScopes = false;
                    ids.Discovery.ShowApiScopes = false;
                });

            if (IdentityServerOptions.CertificateFileName != null)
            {
                var signingCertificate = new X509Certificate2(IdentityServerOptions.CertificateFileName, IdentityServerOptions.CertificatePassword);
                builder.AddSigningCredential(signingCertificate);
            }
            else
            {
                builder.AddTemporarySigningCredential();
            }

            builder.AddInMemoryClients(IdentityServerOptions.Clients);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.ConfigureLogging(Configuration.GetSection(IdentityServerHostConstants.LoggingSectionName));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}