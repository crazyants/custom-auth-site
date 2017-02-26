// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using IdentityServer.Host.Configuration;
using IdentityServer.Host;
using IdentityServer4;
using Microsoft.AspNetCore.Http;

namespace Host
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(env.ContentRootPath)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();

            Configuration = builder.Build();
            
            loggerFactory.ConfigureLogging(Configuration.GetSection(IdentityServerHostConstants.LoggingSectionName));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var options = Configuration.Get<IdentityServerOptions>(IdentityServerHostConstants.IdentityServerOptionsSectionName);
            //options.HttpLoginCallback = ctx =>
            //{
            //    var userId = ctx.Request.Query["userId"].FirstOrDefault();
            //    return Task.FromResult(userId);
            //};
            services.AddCustomIdentityServer(options);

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

            // a cookie is used to track the user's session at IdentityServer
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme,
                LogoutPath = new PathString("/Account/Logout")
            });

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}