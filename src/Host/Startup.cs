// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using IdentityServer4;
using Microsoft.AspNetCore.Http;
using RelativityAuthenticationBridge.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RelativityAuthenticationBridge
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
            
            loggerFactory.ConfigureLogging(Configuration.GetSection(RelativityAuthenticationBridgeConstants.LoggingSectionName));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var options = Configuration.Get<RelativityAuthenticationBridgeOptions>(RelativityAuthenticationBridgeConstants.OptionsSectionName);

            // set the HttpLoginCallback if the identification of the user authentication is 
            // to be determined via a value in the HTTP request itself (such as a header)
            // if this is not set, then the login UI is used. if this is set, then the login UI is disabled.
            //options.HttpLoginCallback = ctx =>
            //{
            //    // this is an example of performing user authentication based
            //    // on something custom in the HTTP request -- perhaps a header value
            //    // that was emitted from an upstream proxy server that has already 
            //    // authenticated the user. 
            //    var userId = ctx.Request.Query["userId"].FirstOrDefault();
            //    return Task.FromResult(userId);
            //};

            services.AddRelativityAuthenticationBridge(options);

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
                LogoutPath = new PathString("/Account/Logout"),
								ExpireTimeSpan = TimeSpan.FromMinutes(2)
            });
						
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}