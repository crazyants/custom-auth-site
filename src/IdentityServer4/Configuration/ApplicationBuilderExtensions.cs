using IdentityServer.Configuration;
using IdentityServer4;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void UsekCuraIdentityServer(this IApplicationBuilder app, IdentityServerRuntimeOptions options)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme,
                ExpireTimeSpan = options.AuthenticationCookieExpiration,
                SlidingExpiration = options.AuthenticationCookieSlidingExpiration,
            });

            app.UseIdentityServer();
        }

        private static string TryGetConfirmedGoogleEmail(JObject user)
        {
            JToken value;
            if (user.TryGetValue("emails", out value))
            {
                var array = JArray.Parse(value.ToString());
                if (array != null && array.Count > 0)
                {
                    foreach (var item in array)
                    {
                        var subObject = JObject.Parse(item.ToString());
                        if (subObject != null)
                        {
                            JToken type;
                            if (subObject.TryGetValue("type", out type))
                            {
                                if (type.ToString() == "account" && subObject.TryGetValue("value", out value))
                                {
                                    return value.ToString();
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

    }
}
