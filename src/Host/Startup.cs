// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using IdentityServer.Configuration;
using IdentityServer.Host;
using IdentityServer4.Filters;

namespace Host
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IdentityServerRuntimeOptions IdentityServerOptions { get; }
        public IdentityServerRuntimeData IdentityServerRuntimeData { get; }
        public Startup(IHostingEnvironment env)
        {
            Configuration = env.BuildConfiguration();
            IdentityServerOptions = Configuration.Get<IdentityServerRuntimeOptions>(IdentityServerHostConstants.IdentityServerRuntimeOptionsSectionName);
            IdentityServerRuntimeData = Configuration.Get<IdentityServerRuntimeData>(IdentityServerHostConstants.IdentityServerRuntimeDataSectionName);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var idSvrBuilder = services.AddkCuraIdentityServer(IdentityServerOptions);
            idSvrBuilder.AddRuntimeData(IdentityServerRuntimeData);

            services.AddMvc(
            options =>
            {
                options.Filters.Add(new ExceptionFilter());
            });
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

            app.UsekCuraIdentityServer(IdentityServerOptions);

            /*
    *           // serilog filter
                Func<LogEvent, bool> serilogFilter = (e) =>
                {
                    var context = e.Properties["SourceContext"].ToString();

                    return (context.StartsWith("\"IdentityServer") ||
                            context.StartsWith("\"IdentityModel") ||
                            e.Level == LogEventLevel.Error ||
                            e.Level == LogEventLevel.Fatal);
                };

                var serilog = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .Enrich.FromLogContext()
                    .Filter.ByIncludingOnly(serilogFilter)
                    .WriteTo.LiterateConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message}{NewLine}{Exception}{NewLine}")
                    .WriteTo.File(@"identityserver4_log.txt")
                    .CreateLogger();

                loggerFactory.AddSerilog(serilog);

            app.UseDeveloperExceptionPage();
                            app.UseIdentityServer();

                        app.UseCookieAuthentication(new CookieAuthenticationOptions
                        {
                            AuthenticationScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                            AutomaticAuthenticate = false,
                            AutomaticChallenge = false
                        });

                        app.UseGoogleAuthentication(new GoogleOptions
                        {
                            AuthenticationScheme = "Google",
                            SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                            ClientId = "708996912208-9m4dkjb5hscn7cjrn5u0r4tbgkbj1fko.apps.googleusercontent.com",
                            ClientSecret = "wdfPY6t8H8cecgjlxud__4Gh"
                        });

                        app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
                        {
                            AuthenticationScheme = "idsrv3",
                            SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                            SignOutScheme = IdentityServerConstants.SignoutScheme,
                            DisplayName = "IdentityServer3",
                            Authority = "https://demo.identityserver.io/",
                            ClientId = "implicit",
                            ResponseType = "id_token",
                            Scope = { "openid profile" },
                            SaveTokens = true,
                            CallbackPath = new PathString("/signin-idsrv3"),
                            SignedOutCallbackPath = new PathString("/signout-callback-idsrv3"),
                            RemoteSignOutPath = new PathString("/signout-idsrv3"),
                            TokenValidationParameters = new TokenValidationParameters
                            {
                                NameClaimType = "name",
                                RoleClaimType = "role"
                            }
                        });

                        app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
                        {
                            AuthenticationScheme = "aad",
                            SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                            SignOutScheme = IdentityServerConstants.SignoutScheme,
                            DisplayName = "AAD",
                            Authority = "https://login.windows.net/4ca9cb4c-5e5f-4be9-b700-c532992a3705",
                            ClientId = "96e3c53e-01cb-4244-b658-a42164cb67a9",
                            ResponseType = "id_token",
                            Scope = { "openid profile" },
                            CallbackPath = new PathString("/signin-aad"),
                            SignedOutCallbackPath = new PathString("/signout-callback-aad"),
                            RemoteSignOutPath = new PathString("/signout-aad"),
                            TokenValidationParameters = new TokenValidationParameters
                            {
                                NameClaimType = "name",
                                RoleClaimType = "role"
                            }
                        });

                        app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
                        {
                            AuthenticationScheme = "adfs",
                            SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                            SignOutScheme = IdentityServerConstants.SignoutScheme,
                            DisplayName = "ADFS",
                            Authority = "https://adfs.leastprivilege.vm/adfs",
                            ClientId = "c0ea8d99-f1e7-43b0-a100-7dee3f2e5c3c",
                            ResponseType = "id_token",
                            Scope = { "openid profile" },
                            CallbackPath = new PathString("/signin-adfs"),
                            SignedOutCallbackPath = new PathString("/signout-callback-adfs"),
                            RemoteSignOutPath = new PathString("/signout-adfs"),
                            TokenValidationParameters = new TokenValidationParameters
                            {
                                NameClaimType = "name",
                                RoleClaimType = "role"
                            }
                        });  */

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}